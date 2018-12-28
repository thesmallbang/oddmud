using System;
using System.Collections.Generic;
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

            // // this whole thing can be optimized to send way smaller replacement segments later

            // var combatView = MudLikeOperationBuilder.Start($"enc_{encounter.Id}")
            //      .StartContainer($"enc_{encounter.Id}")
            //      .AddTextLine("");

            // var gridEncounter = (GridEncounter)encounter;
            // var entityAction = (GridTargetAction)action;

            // //var dmgDone = encounter.ActionLog.Select((a) => (ICombatAction<GridEntity>)a)
            // //      .GroupBy(a => new { a.SourceEntity })
            // //      .Select(a => new { Attacker = a.Key.SourceEntity, Damage = a.Sum(s => s.DamageDone) })
            // //  .ToList();

            // //var dmgTaken = encounter.ActionLog.Select((a) => (GridSingleTargetAction)a)
            // //                            .GroupBy(a => new { a.TargetEntity })
            // //                            .Select(a => new { Attacked = a.Key.TargetEntity, Damage = a.Sum(s => s.DamageDone) })
            // //                        .ToList();

            // foreach (var factionName in gridEncounter.Factions.Keys)
            // {
            //     var factionEntities = gridEncounter.Factions[factionName];


            //     foreach (var entity in factionEntities)
            //     {
            //         combatView
            //           .AddText(entity.Name, (entity.IsAlive && !encounter.Dead.Contains(entity) ? TextColor.Normal : TextColor.Red));
            //         if (entity.IsAlive && !encounter.Dead.Contains(entity))
            //         {
            //             combatView
            //             .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "health")?.Value}", TextColor.Green)
            //             .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "mana")?.Value}", TextColor.Blue)
            //             .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "stamina")?.Value}", TextColor.Yellow)
            //           ;
            //         }
            //         else
            //         {
            //             combatView.AddText(" Dead", TextColor.Red, TextSize.Small);
            //         }

            //         //var entityDmgDone = dmgDone.FirstOrDefault(d => d.Attacker == entity);
            //         //if (entityDmgDone != null)
            //         //{
            //         //    combatView
            //         //        .AddText($" Dmg {entityDmgDone.Damage}", TextColor.Teal);
            //         //    ;
            //         //}
            //         //var entityDmgTaken = dmgTaken.FirstOrDefault(d => d.Attacked == entity);
            //         //if (entityDmgTaken != null)
            //         //{
            //         //    combatView
            //         //        .AddText($" Taken {entityDmgTaken.Damage}", TextColor.Teal);
            //         //    ;
            //         //}

            //         combatView.AddLineBreak();

            //     }

            //     combatView.AddTextLine("---------------------------");

            // }


            // // add last X actions 
            // var actions = encounter.ActionLog.OrderByDescending(a => a.ExecutedTime).Take(10).OrderBy(a => a.ExecutedTime).ToList();
            // actions.ForEach((a) => a.AppendToOperation(combatView));

            // combatView.EndContainer($"enc_{encounter.Id}");

            // var view = MudLikeViewBuilder.Start()
            //.AddOperation(combatView.Build()
            //).Build();


            // if (entityAction.TargetEntities.Contains(entityAction.SourceEntity) || entityAction.TargetEntities.Count == 0)
            //     await Game.Network.SendViewCommandsToMapAsync(entityAction.SourceEntity.Map, view);
            // else
            // {
            //     var maps = new List<IMap>() { entityAction.SourceEntity.Map };
            //     maps.AddRange(entityAction.TargetEntities.Where(t => t.Map != entityAction.SourceEntity.Map).Select(t => t.Map).Distinct());

            //     maps.ForEach(async m => await Game.Network.SendViewCommandsToMapAsync(m, view));

            // }


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



            foreach (var faction in factionInfo)
            {
                var players = faction.FactionEntities.Where(e => e.IsPlayer()).Select(e => e).Distinct();

                if (!players.Any())
                    continue;

                //    var combatView = MudLikeOperationBuilder.Start($"enc_{encounter.Id}")
                //   .StartContainer($"enc_{encounter.Id}").AddLineBreak();

                //    foreach (var entity in faction.FactionEntities)
                //    {
                //        var experienceStat = entity.Stats.FirstOrDefault(s => s.Name == "experience");

                //        combatView
                //            .AddText(entity.Name, (entity.IsAlive && !encounter.Dead.Contains(entity) ? TextColor.Normal : TextColor.Red));
                //        if (entity.IsAlive && !encounter.Dead.Contains(entity))
                //        {
                //            combatView
                //            .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "health")?.Value}", TextColor.Green)
                //            .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "mana")?.Value}", TextColor.Blue)
                //            .AddText($" {entity.Stats.FirstOrDefault(s => s.Name == "stamina")?.Value}", TextColor.Yellow)
                //            .AddText($" Lv {experienceStat.Value}/{experienceStat.Base}", TextColor.Yellow)
                //          ;
                //        }
                //        else
                //        {
                //            combatView.AddText(" Dead", TextColor.Red, TextSize.Small);
                //        }

                //        combatView.AddLineBreak();

                //    }


                //    if (faction.isWinner)
                //        combatView.AddTextLine($"You earned {experience} experience");
                //    else
                //        combatView.AddTextLine($"You lost the encounter.");

                //    combatView.EndContainer($"enc_{encounter.Id}");


                //    var view = MudLikeViewBuilder.Start().AddOperation(combatView.Build()).Build();

                //    await Game.Network.SendViewCommandsToPlayersAsync(players.Cast<IPlayer>(), view);


            }

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
