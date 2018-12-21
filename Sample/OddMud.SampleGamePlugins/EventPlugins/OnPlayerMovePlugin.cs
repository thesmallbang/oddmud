using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.SampleGame.Misc;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OddMud.Core.Game;

namespace OddMud.SampleGamePlugins.EventPlugins
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

                var leftBehindNotification = new MudLikeCommandBuilder()
                    .AddPlayers(playersLeftBehind)
                        .Build(ViewCommandType.Replace, "playerlist");
                await Game.Network.SendViewCommandsToMapAsync(e.OldMap, leftBehindNotification);
            }


            var map = (GridMap)e.NewMap;
            await Game.Network.AddPlayerToMapGroupAsync(e.Player, map);

            var player = e.Player;
            var lookView = MudLikeCommandBuilder.Start()
                .AddWorldDate(Game.World.Time.WorldTime)
                .AddMap(map)
                .Build(ViewCommandType.Set);
            await Game.Network.SendViewCommandsToPlayerAsync(player, lookView);


            // update the map with a new playerslist
            var playersUpdate = new MudLikeCommandBuilder().AddPlayers(map.Players)
                 .Build(ViewCommandType.Replace, "playerlist");
            await Game.Network.SendViewCommandsToMapAsync(map, playersUpdate);

            var itemsUpdate = new MudLikeCommandBuilder().AddItems(map.Items)
                .Build(ViewCommandType.Replace, "itemlist");
            await Game.Network.SendViewCommandsToMapAsync(map, itemsUpdate);

            var entitiesUpdate = new MudLikeCommandBuilder().AddEntities(map.Entities)
                            .Build(ViewCommandType.Replace, MudContainers.EntityList.ToString());
            await Game.Network.SendViewCommandsToMapAsync(map, entitiesUpdate);


        }
    }
}
