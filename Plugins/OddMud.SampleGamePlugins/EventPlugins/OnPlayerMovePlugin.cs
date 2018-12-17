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

        public void Configure(IGame game, IServiceProvider serviceProvider)
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

                var playersLeftBehind = e.OldMap.Players.Except(e.Player);

                var leftBehindNotification = new MudLikeCommandBuilder().GetPlayersUpdate(playersLeftBehind)
               .AddTextLine($" -{e.Player.Name}", TextColor.Red)
                .Build(ViewCommandType.Replace);

                await Game.Network.SendViewCommandsToMapAsync(e.OldMap, leftBehindNotification);
            }


            var map = (GridMap)e.NewMap;

            // display the map information to the player
            var mapView = new MudLikeCommandBuilder()
                .AddTextLine(map.ToString(), TextColor.Teal, TextSize.Strong)
                .AddTextLine(map.Description, size: TextSize.Large)
                .AddText("exits: ")
                .AddTextLine(string.Join(",", map.Exits.Select(o => o.ToString().ToLower())), TextColor.Green)
                .Build();

            // tell the other players about the player who joined their map
            var existingPlayers = map.Players.Except(e.Player);
            var playersUpdate = new MudLikeCommandBuilder().GetPlayersUpdate(existingPlayers)
                .AddTextLine($" +{e.Player.Name}", TextColor.Green)
                 .Build(ViewCommandType.Replace);

            var worldDateView = MudLikeCommandBuilder.Start()
                .AddWorldDate(Game.World.Time.WorldTime)
                .Build(ViewCommandType.Replace);

            await Game.Network.AddPlayerToMapGroupAsync(e.Player, map);
            await Game.Network.SendViewCommandsToPlayerAsync(e.Player, mapView);
            await Game.Network.SendViewCommandsToPlayerAsync(e.Player, worldDateView);

            await Game.Network.SendViewCommandsToMapExceptAsync(map, e.Player, playersUpdate);
            await Game.Network.SendViewCommandsToPlayerAsync(e.Player, playersUpdate);
        }

    }
}
