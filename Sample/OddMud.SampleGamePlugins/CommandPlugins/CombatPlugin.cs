using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Extensions;
using OddMud.SampleGame.GameModules;
using OddMud.View.MudLike;

namespace OddMud.SampleGamePlugins.CommandPlugins
{



    public class InitiateCombatParserOptions
    {
        [Option('n', "name", Required = false, HelpText = "Name of the entity to attack.")]
        public IEnumerable<string> Name { get; set; }

        [Option('a', "any", Required = false, HelpText = "any anything?", Default = false)]
        public bool AttackAny { get; set; }

    }

    public class CombatPlugin : LoggedInCommandPlugin
    {
        private ILogger<CombatPlugin> _logger;
        private CombatModule _combatModule;

        public new GridGame Game => (GridGame)base.Game;
        public override IReadOnlyList<string> Handles => new List<string>() { "attack" };

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

            await Game.Network.SendViewCommandsToMapAsync(map,
               MudLikeViewBuilder.Start()
               .AddOperation(
                   MudLikeOperationBuilder.Start($"enc_{encounter.Id}")
                        .StartContainer($"enc_{encounter.Id}")
                        .AddTextLine("An encounter has begun")
                        .EndContainer($"enc_{encounter.Id}")
                        .Build()
                   )
               .Build());

        }

        public override async Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            await base.LoggedInProcessAsync(request, player);
            switch (request.Data.FirstPart)
            {
                case "attack":
                    await ProcessBasicAttack(request, player);
                    break;
            }

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

        private async  Task Encounter_ActionExecuted(IEncounter encounter, ICombatAction action)
        {

            var combatView  = MudLikeOperationBuilder.Start(ViewOperationType.Append, $"enc_{encounter.Id}");
            action.AppendToOperation(combatView);

            var view = MudLikeViewBuilder.Start()
           .AddOperation(combatView.Build()
           ).Build();


            await Game.Network.SendViewCommandsToMapAsync(encounter.Combatants.Keys.First().Map, view);


        }

        private async Task Encounter_Ended(IEncounter encounter, EncounterEndings arg2)
        {

            encounter.ActionExecuted -= Encounter_ActionExecuted;
            encounter.Ended -= Encounter_Ended;

            var map = ((GridPlayer)encounter.Combatants.First().Key).Map;
            


            // build some sort of encounter ending
            var victors = encounter.Combatants.Keys.Where(k => k.IsAlive).Select(o => (GridEntity)o).ToList();
            


            var view = MudLikeViewBuilder.Start()
             .AddOperation(
                      MudLikeOperationBuilder.Start($"enc_{encounter.Id}")
                            .StartContainer($"enc_{encounter.Id}")
                            .AddTextLine("Result : Completed")
                            .EndContainer($"enc_{encounter.Id}")
                            .Build()
             ).Build();


            await Game.Network.SendViewCommandsToMapAsync(map, view);





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
