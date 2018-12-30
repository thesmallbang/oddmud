using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Logging;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Extensions;
using OddMud.SampleGame.GameModules;
using OddMud.SampleGame.GameModules.Combat;
using OddMud.SampleGame.ViewComponents;
using OddMud.View.ComponentBased;

namespace OddMud.SampleGamePlugins.CommandPlugins
{



    public class InitiateCombatParserOptions
    {
        [Option('n', "name", Required = false, HelpText = "Name of the entity to attack.")]
        public IEnumerable<string> Name { get; set; }

        [Option('a', "any", Required = false, HelpText = "any anything?", Default = false)]
        public bool AttackAny { get; set; }

        [Option('s', "skip", Required = false, HelpText = "skip?", Default = 0)]
        public int Skip { get; set; }

    }

    public class CastingParserOptions
    {
        [Option('n', "name", Required = false, HelpText = "Name of the ability to use")]
        public IEnumerable<string> Name { get; set; }

        [Option("askip", Required = false, HelpText = "How many abilities matching that name to skip", Default = 0)]

        public int AbiitySkip { get; set; }

        [Option('a', "any", Required = false, HelpText = "any anything?", Default = false)]
        public bool AttackAny { get; set; }

        [Option('s', "skip", Required = false, HelpText = "skip?", Default = 0)]
        public int Skip { get; set; }

    }

    public class CombatPlugin : LoggedInCommandPlugin
    {
        private ILogger<CombatPlugin> _logger;
        private CombatModule _combatModule;

        public new GridGame Game => (GridGame)base.Game;
        public override IReadOnlyList<string> Handles => new List<string>() { "attack", "cast" };

        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
            _logger = (ILogger<CombatPlugin>)serviceProvider.GetService(typeof(ILogger<CombatPlugin>));
            _combatModule = (CombatModule)serviceProvider.GetService(typeof(IGameModule<CombatModule>));

            _combatModule.AddedEncounter += HandleNewEncounter;
            _combatModule.RemovedEncounter += HandleRemoveEncounter;


        }

        private Task HandleRemoveEncounter(IEncounter encounter)
        {
            encounter.ActionExecuted -= Encounter_ActionExecuted;
            encounter.Ended -= Encounter_Ended;

            return Task.CompletedTask;
        }

        private async Task HandleNewEncounter(IEncounter encounter)
        {
            encounter.ActionExecuted += Encounter_ActionExecuted;
            encounter.Ended += Encounter_Ended;

            var map = ((GridPlayer)encounter.Combatants.First().Key).Map;

            //await Game.Network.SendViewCommandsToMapAsync(map,
            //   MudLikeViewBuilder.Start()
            //   .AddOperation(
            //       MudLikeOperationBuilder.Start($"enc_{encounter.Id}")
            //            .StartContainer($"enc_{encounter.Id}")
            //            .AddTextLine("Combat Starting")
            //            .EndContainer($"enc_{encounter.Id}")
            //            .Build()
            //       )
            //   .Build());

        }

        public override async Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            await base.LoggedInProcessAsync(request, player);
            switch (request.Data.FirstPart)
            {
                case "attack":
                    await ProcessBasicAttack(request, player);
                    break;
                case "cast":
                    await ProcessBasicCast(request, player);
                    break;
            }

        }

        private Task ProcessBasicCast(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<CastingParserOptions>(request.Data.StringFrom(1).Split(' '))
               .WithParsed(async (parsed) =>
               {

                   var entityName = string.Join(" ", parsed.Name);

                   if (!parsed.AttackAny && string.IsNullOrWhiteSpace(entityName))
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, GetHelp());
                       return;
                   }


                   GridEntity target = (GridEntity)(
                        parsed.AttackAny ?
                            player.Map.Entities :
                            player.Map.Entities.Where(mi => mi.Name.IndexOf(entityName, StringComparison.OrdinalIgnoreCase) >= 0))
                            .Skip(parsed.Skip)
                   .FirstOrDefault();

                   if (target == null)
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, "No matching target found");
                       return;
                   }

                   entityName = target.Name;

                   if (!target.IsAttackable())
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, $"{entityName} is not attackable");
                       return;
                   }

                   var issues = string.Empty;

                   var encounter = await _combatModule.AppendOrNewEncounterAsync((GridEntity)player, target);



               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, GetHelp());

               })
               ;

            return Task.CompletedTask;
        }

        private Task ProcessBasicAttack(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<InitiateCombatParserOptions>(request.Data.StringFrom(1).Split(' '))
               .WithParsed(async (parsed) =>
               {

                   var entityName = string.Join(" ", parsed.Name);

                   if (!parsed.AttackAny && string.IsNullOrWhiteSpace(entityName))
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, GetHelp());
                       return;
                   }


                   GridEntity target = (GridEntity)(
                        parsed.AttackAny ?
                            player.Map.Entities :
                            player.Map.Entities.Where(mi => mi.Name.IndexOf(entityName, StringComparison.OrdinalIgnoreCase) >= 0))
                            .Skip(parsed.Skip)
                   .FirstOrDefault();

                   if (target == null)
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, "No matching target found");
                       return;
                   }

                   entityName = target.Name;

                   if (!target.IsAttackable())
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, $"{entityName} is not attackable");
                       return;
                   }

                   var issues = string.Empty;

                   var encounter = await _combatModule.AppendOrNewEncounterAsync((GridEntity)player, target);



               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, GetHelp());

               })
               ;

            return Task.CompletedTask;

        }

        private async Task Encounter_ActionExecuted(IEncounter encounter, ICombatAction action)
        {
            var gridEncounter = (GridEncounter)encounter;

            var encounterData = new EncounterData() { Id = encounter.Id, Status = "Fighting" };
            var viewBuilder = ComponentViewBuilder<ComponentTypes>.Start()
            .AddComponent(ComponentTypes.EncounterData, encounterData)
            ;

            var info = new List<string>() { "health", "mana", "stamina", "level" };
            foreach (GridEntity entity in gridEncounter.Combatants.Keys.ToList())
            {
                var stats = entity.Stats.Where(s => info.Contains(s.Name)).ToList();
                var entityData = new PlayerData() { Id = entity.Id, Name = entity.Name };
                encounterData.Entities.Add(entityData);
                foreach (BasicStat stat in stats)
                {
                    Debug.WriteLine(stat.Name);

                    switch (stat.Name)
                    {
                        case "health":
                            var currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));
                            entityData.Health = currentStatPercent;
                            if (entityData.Health <= 0)
                                entityData.Name += " (Dead)";
                            break;
                        case "mana":
                            currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));

                            entityData.Mana = currentStatPercent;
                            break;
                        case "stamina":
                            currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));

                            entityData.Stamina = currentStatPercent;
                            break;
                        case "level":
                            entityData.Level = stat.Value;
                            break;
                    }
                }

            }
            Debug.WriteLine("done loop");
            var players = gridEncounter.Combatants.Keys.Where(e => e.IsPlayer()).Select(e => (IPlayer)e).ToList();

            await Game.Network.SendViewCommandsToPlayersAsync(players, viewBuilder);


        }

        private async Task Encounter_Ended(IEncounter encounter, EncounterEndings ending)
        {

            encounter.ActionExecuted -= Encounter_ActionExecuted;
            encounter.Ended -= Encounter_Ended;

            if (ending == EncounterEndings.Expired)
                return;

            var gridEncounter = (GridEncounter)encounter;


            // allocate experience to winners

            var factionInfo = gridEncounter.Factions.Select(f => new
            {
                FactionName = f.Key,
                FactionEntities = f.Value,
                isWinner = f.Value.Count(e => !gridEncounter.Dead.Contains(e)) > 0,
                FactionSize = f.Value.Count,
                AverageLevel = f.Value.Average(e => e.Stats.FirstOrDefault(s => s.Name == "level")?.Value).GetValueOrDefault(0),
                MinLevel = f.Value.Min(e => e.Stats.FirstOrDefault(s => s.Name == "level")?.Value).GetValueOrDefault(0),
                MaxLevel = f.Value.Max(e => e.Stats.FirstOrDefault(s => s.Name == "level")?.Value).GetValueOrDefault(0)
            }).ToList();


            // this would need updated to support more than 2 factions properly
            var winners = factionInfo.FirstOrDefault(f => f.isWinner);
            var losers = factionInfo.FirstOrDefault(f => !f.isWinner);

            var cancelExperience = false;

            if (winners.MinLevel < winners.MaxLevel - 5)
                cancelExperience = true;


            var sizeScaler = 1.5 * (winners.FactionSize - losers.FactionSize);
            var experienceScaler = 1 + (losers.AverageLevel - winners.AverageLevel) * 0.2;
            var partialScaled = (3 * losers.AverageLevel) * experienceScaler;
            var experience = partialScaled - sizeScaler;

            if (!cancelExperience)
            {
                foreach (var winner in winners.FactionEntities)
                {
                    // players and monsters both gain experience the same for now
                    var experienceStat = (BasicStat)winner.Stats.FirstOrDefault(s => s.Name == "experience");
                    await experienceStat?.ApplyAsync(Convert.ToInt32(experience));

                    if (experienceStat != null)
                    {
                        // check if leveled
                        if (experienceStat.Value == experienceStat.Base)
                        {
                            var levelStat = winner.Stats.FirstOrDefault(s => s.Name == "level");
                            await levelStat?.ApplyAsync(1);
                            await experienceStat.RebaseAsync(Convert.ToInt32(experienceStat.Base * 1.5), 0);

                            // fill stats
                            var vitalStats = new List<string>() { "health", "mana", "stamina" };
                            foreach (BasicStat stat in winner.Stats.Where(s => vitalStats.Contains(s.Name)).ToList())
                            {
                                await stat.Fill();
                            }

                        }
                    }

                }
            }



            var encounterData = new EncounterData() { Id = encounter.Id, Status = "Ended" };
            var viewBuilder = ComponentViewBuilder<ComponentTypes>.Start()
            .AddComponent(ComponentTypes.EncounterData, encounterData)
            ;

            var info = new List<string>() { "health", "mana", "stamina", "level" };
            foreach (GridEntity entity in gridEncounter.Combatants.Keys.ToList())
            {
                var stats = entity.Stats.Where(s => info.Contains(s.Name)).ToList();
                var entityData = new PlayerData() { Id = entity.Id, Name = entity.Name };
                encounterData.Entities.Add(entityData);
                foreach (BasicStat stat in stats)
                {
                    Debug.WriteLine(stat.Name);

                    switch (stat.Name)
                    {
                        case "health":
                            var currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));

                            entityData.Health = currentStatPercent;
                            if (entityData.Health <= 0)
                            {
                                entityData.Name += " (Dead)";
                            }
                            break;
                        case "mana":
                            currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));

                            entityData.Mana = currentStatPercent;
                            break;
                        case "stamina":
                            currentStatPercent = Convert.ToInt32(((double)stat.Value / (double)stat.Base * 100));

                            entityData.Stamina = currentStatPercent;
                            break;
                        case "level":
                            entityData.Level = stat.Value;
                            break;
                    }
                }

            }
            var players = gridEncounter.Combatants.Keys.Where(e => e.IsPlayer()).Select(e => (IPlayer)e).ToList();

            await Game.Network.SendViewCommandsToPlayersAsync(players, viewBuilder);


        }

        public string GetHelp()
        {
            _logger.LogInformation("Getting help..");
            var output = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                var parser = new Parser(config => config.HelpWriter = stringWriter);
                var result = parser.ParseArguments<InitiateCombatParserOptions>(new List<string>() { "attack", "--help" });
                output = stringWriter.ToString();
            }
            _logger.LogInformation($"Output: {output}");
            // cleanup the output which includes too much
            var starter = output.IndexOf('-');
            var ender = output.IndexOf("--help");
            return output.Substring(starter, ender - starter);
        }
    }
}
