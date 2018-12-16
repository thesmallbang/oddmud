using OddMud.BasicGame;
using OddMud.BasicGame.Extensions;
using OddMud.BasicGame.Misc;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins.EventPlugins
{
    public class OnPlayerMovePlugin : IEventPlugin
    {

        public string Name => nameof(OnPlayerMovePlugin);
        public GridGame Game;

        public void Configure(IGame game)
        {
            Game = (GridGame)game;
            Game.World.PlayerMoved += HandleMapChanged;
        }

        private async Task HandleMapChanged(Object sender, IMapChangeEvent e)
        {

            if (e.OldMap != null)
            {
                await e.OldMap.RemovePlayerAsync(e.Player);
                await Game.Network.RemovePlayerFromMapGroupAsync(e.Player, e.OldMap);
                await Game.Network.SendMessageToMapAsync(e.OldMap, $"{e.Player.Name} has left the area.");
            }


            var map = (GridMap)e.NewMap;

            // display the map information to the player
            var view = new List<IViewItem>();
            view.Add(new TextItem(map.ToString(), TextColor.Teal, TextSize.Strong));
            view.Add(new LineBreakItem());
            view.Add(new TextItem(map.Description, TextSize.Large));
            view.Add(new LineBreakItem());
            view.Add(new TextItem("exits: "));
            view.Add(new TextItem(string.Join("," , map.Exits.Select(o=> o.ToString())), TextColor.Yellow));
            view.Add(new LineBreakItem());

            await Game.Network.SendViewCommandsToPlayerAsync(e.Player, new MudViewCommands(view));
            await Game.Network.AddPlayerToMapGroupAsync(e.Player, e.NewMap);
            await Game.Network.SendMessageToMapExceptAsync(e.NewMap, e.Player, $"{e.Player.Name} has joined the area.");


        }

    }
}
