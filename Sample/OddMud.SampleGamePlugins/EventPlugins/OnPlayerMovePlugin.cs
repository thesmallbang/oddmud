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

                var leftBehindNotification = MudLikeOperationBuilder.Start("playerlist")
                    .AddPlayers(playersLeftBehind)
                        .Build();
                await Game.Network.SendViewCommandsToMapAsync(e.OldMap, MudLikeViewBuilder.Start().AddOperation(leftBehindNotification).Build());
            }


            var map = (GridMap)e.NewMap;
            await Game.Network.AddPlayerToMapGroupAsync(e.Player, map);

            var player = e.Player;
            var lookView = MudLikeOperationBuilder.Start()
                .AddWorldDate(Game.World.Time.WorldTime)
                .AddMap(map)
                .Build();


            await Game.Network.SendViewCommandsToPlayerAsync(player, MudLikeViewBuilder.Start().AddOperation(lookView).Build());


            // update the map with a new playerslist
            var playersUpdate = MudLikeOperationBuilder.Start("playerlist").AddPlayers(map.Players)
                 .Build();

            var itemsUpdate = MudLikeOperationBuilder.Start("itemlist").AddItems(map.Items)
                .Build();

            var entitiesUpdate = MudLikeOperationBuilder.Start(MudContainers.EntityList.ToString()).AddEntities(map.Entities)
                 .Build();

            var update = MudLikeViewBuilder.Start().AddOperation(playersUpdate).AddOperation(itemsUpdate).AddOperation(entitiesUpdate).Build();

            await Game.Network.SendViewCommandsToMapAsync(map, update);


        }
    }
}
