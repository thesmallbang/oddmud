using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Extensions;
using OddMud.View.MudLike;

namespace OddMud.SampleGamePlugins.CommandPlugins
{


    public class ItemPickupParserOptions
    {
        [Option('n', "name", Required = false, HelpText = "Name of the item.")]
        public IEnumerable<string> Name { get; set; }

        [Option('q', "quantity", Required = false, HelpText = "how many to pickup", Default = 1)]
        public int Quantity { get; set; }

        [Option('a', "any", Required = false, HelpText = "pickup anything?", Default = false)]
        public bool PickupAny { get; set; }

    }

    public class MapCommonPlugin : LoggedInCommandPlugin
    {

        public new GridGame Game => (GridGame)base.Game;
        public override IReadOnlyList<string> Handles => new List<string>() { "item" };

        public override async Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            await base.LoggedInProcessAsync(request, player);

            switch (request.Data.SecondPart)
            {
                case "pickup":
                    await ProcessItemPickupAsync(request, player);
                    break;
            }

        }

        private Task ProcessItemPickupAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<ItemPickupParserOptions>(request.Data.StringFrom(2).Split(' '))
               .WithParsed(async (parsed) =>
               {

                   var itemName = string.Join(" ", parsed.Name);

                   // take each matching item up to the quantity we requested
                   var mapItems = (
                        parsed.PickupAny ? 
                            player.Map.Items :
                            player.Map.Items.Where(mi => mi.Name.IndexOf(itemName, StringComparison.OrdinalIgnoreCase) >= 0))
                   .Take(parsed.Quantity)
                   .ToList();

                   if (mapItems.Count == 0)
                   {
                       await Game.Network.SendMessageToPlayerAsync(player, "Matching item not found");
                       return;
                   }

                   itemName = mapItems[0].Name;

                   await Game.Network.SendMessageToPlayerAsync(player, $"Looting {itemName} | quantity {mapItems.Count}");
                   foreach (var i in mapItems)
                   {
                       // double check one last time the item wasn't picked up elsewhere in the time we've been picking up others
                       if (player.Map.Items.Contains(i))
                           await player.PickupItemAsync(Game, i);
                   }

                   var itemView = MudLikeViewBuilder.Start()
                    .AddOperation(
                        MudLikeOperationBuilder.Start("itemlist").AddItems(player.Map.Items).Build())
                    .Build();


                   await Game.Network.SendViewCommandsToMapAsync(player.Map, itemView);

               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, "invalid command(pickup) - for help type item help");
               });

            return Task.CompletedTask;

        }
    }
}
