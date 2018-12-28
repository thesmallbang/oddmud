using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.SampleGamePlugins;
using CommandLine;
using OddMud.SampleGame.Misc;
using System.Linq;
using System.Diagnostics;
using OddMud.SampleGame.GameModules.Combat;

namespace OddMud.SampleGamePlugins.CommandPlugins
{




    public class ListSpawnerParserOptions
    {

        [Option('t', "type", Required = false, HelpText = "enemy or item")]
        public string SpawnType { get; set; }

    }

    public class AddSpawnerParserOptions
    {

        [Option('t', "type", Required = false, HelpText = "enemy or item")]
        public string SpawnType { get; set; }

        [Option('i', "id", Required = false, HelpText = "enemy or item id")]
        public string EntityId { get; set; }

        [Option('m', "mapid", Required = false, HelpText = "map")]
        public int? MapId { get; set; }

    }

    public class SpawnBuilderPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(SpawnBuilderPlugin);
        public new GridGame Game => (GridGame)base.Game;
        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "spawner" };

        public override Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            switch (request.Data.SecondPart)
            {
                case "list":
                    return ProcessListSpawnersAsync(request, player);
                case "add":
                    return ProcessAddSpawnersAsync(request, player);

            }

            return base.LoggedInProcessAsync(request, player);
        }

        private Task ProcessListSpawnersAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<ListSpawnerParserOptions>(request.Data.StringFrom(2).Split(' '))
               .WithParsed(async (parsed) =>
               {
                   var validSpawnTypes = new List<string>() { "item", "entity" };


                   var match = validSpawnTypes.FirstOrDefault(s => s == parsed.SpawnType);
                   if (match != null)
                   {
                       validSpawnTypes.RemoveAll(s => s != match);
                   }


                   //var viewbuilder = MudLikeViewBuilder.Start();

                   //var partial = Game.Spawners.Cast<GridSpawner>().Where(spawner => spawner.Map == player.Map).ToList();
                   //foreach (var spawnType in validSpawnTypes)
                   //{
                   //    IViewOperation<IViewItem> operation = null;
                   //    var opBuilder = MudLikeOperationBuilder.Start();
                   //    opBuilder
                   //    .AddTextLine(spawnType, TextColor.Olive)
                   //    .AddTextLine("------------------------------", TextColor.Olive);

                   //    switch (spawnType)
                   //    {
                   //        case "entity":
                   //            var entityMatches = partial.Where(spawner => spawner.SpawnType == Core.Game.SpawnType.Entity).Select(s => s.EntityId).ToList();
                   //            var entities = Game.Entities.Where(e => entityMatches.Contains(e.Id)).ToList();
                   //            foreach (GridEntity entity in entities)
                   //            {
                   //                opBuilder
                   //                .AddText($"{entity.Id} ", TextColor.Aqua)
                   //                .AddText($"{entity.Name} ");


                   //                entity.Stats.ToList().ForEach(stat =>
                   //                {
                   //                    opBuilder
                   //                    .AddText($"{stat.Name} ", TextColor.Gray)
                   //                    .AddText($"{stat.Value} ")
                   //                    ;
                   //                });
                   //                opBuilder.AddLineBreak();
                   //            }

                   //            operation = opBuilder.Build();

                   //            break;
                   //        case "item":
                   //            var itemMatches = partial.Where(spawner => spawner.SpawnType == Core.Game.SpawnType.Item);
                   //            var items = Game.Items.Where(e => itemMatches.Select(i => i.Id).Contains(e.Id)).ToList();

                   //            foreach (GridItem item in items)
                   //            {
                   //                opBuilder
                   //                .AddText($"{item.Id} ", TextColor.Aqua)
                   //                .AddText($"{item.Name} ")
                   //                ;

                   //                item.Stats.ToList().ForEach(stat =>
                   //                {
                   //                    opBuilder
                   //                    .AddText($"{stat.Name} ", TextColor.Gray)
                   //                    .AddText($"{stat.Value} ")
                   //                    ;
                   //                });
                   //                opBuilder.AddLineBreak();
                   //            }

                   //            operation = opBuilder.Build();
                   //            break;
                   //        default:
                   //            break;
                   //    }

                   //    if (operation != null)
                   //        viewbuilder.AddOperation(operation);


                   //}


                   //await Game.Network.SendViewCommandsToPlayerAsync(player, viewbuilder.Build());


               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type spawner help");
               });

            return Task.CompletedTask;

        }

        private Task ProcessAddSpawnersAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<AddSpawnerParserOptions>(request.Data.StringFrom(2).Split(' '))
               .WithParsed(async (parsed) =>
               {
                   var validSpawnTypes = new List<string>() { "item", "entity" };


                   var match = validSpawnTypes.FirstOrDefault(s => s == parsed.SpawnType);
                   if (match != null)
                   {
                       validSpawnTypes.RemoveAll(s => s != match);
                   }

                   if (validSpawnTypes.Count > 1)
                       validSpawnTypes.RemoveAt(1);


                   


               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type spawner help");
               });

            return Task.CompletedTask;

        }



    }
}
