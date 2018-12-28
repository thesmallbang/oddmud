using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.SampleGame.Misc;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.View.ComponentBased;
using OddMud.SampleGame.ViewComponents;

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

                e.OldMap.PlayersChanged -= MapPlayersChanged;
                e.OldMap.ItemsChanged -= MapItemsChanged;
                e.OldMap.EntitiesChanged -= MapEntitiesChanged;


                //var playersLeftBehind = e.OldMap.Players.Except(e.Player);

                //var leftBehindNotification = MudLikeOperationBuilder.Start("playerlist")
                //    .AddPlayers(playersLeftBehind)
                //        .Build();
                //await Game.Network.SendViewCommandsToMapAsync(e.OldMap, MudLikeViewBuilder.Start().AddOperation(leftBehindNotification).Build());
            }

            var map = (GridMap)e.NewMap;
            await Game.Network.AddPlayerToMapGroupAsync(e.Player, map);

            map.PlayersChanged += MapPlayersChanged;
            map.ItemsChanged += MapItemsChanged; ;
            map.EntitiesChanged += MapEntitiesChanged; ;


            var builder = ComponentViewBuilder<ComponentTypes>.Start()
                .AddComponent(ComponentTypes.MapData, new WorldComponentData()
                {
                    Id = map.Id,
                    Title = map.Name,
                    Description = map.Description,
                    Exits = map.Exits.Select(ex => ex.ToString().ToLower()).ToList()
                })
                ;

            await Game.Network.SendViewCommandsToMapAsync(map, builder);

        }

        private async Task MapEntitiesChanged(IMap map, IReadOnlyList<IEntity> entities)
        {
            //var entitiesUpdate = MudLikeOperationBuilder.Start(MudContainers.EntityList.ToString()).AddEntities(entities)
            //    .Build();
            //await Game.Network.SendViewCommandsToMapAsync(map, MudLikeViewBuilder.Start().AddOperation(entitiesUpdate).Build());
        }

        private async Task MapItemsChanged(IMap map, IReadOnlyList<IItem> arg)
        {
            //var itemsUpdate = MudLikeOperationBuilder.Start("itemlist").AddItems(map.Items)
            //  .Build();
            //await Game.Network.SendViewCommandsToMapAsync(map, MudLikeViewBuilder.Start().AddOperation(itemsUpdate).Build());
        }

        private async Task MapPlayersChanged(IMap map, IReadOnlyList<IPlayer> arg)
        {
            //var playersUpdate = MudLikeOperationBuilder.Start("playerlist").AddPlayers(map.Players)
            //    .Build();
            //await Game.Network.SendViewCommandsToMapAsync(map, MudLikeViewBuilder.Start().AddOperation(playersUpdate).Build());
        }
    }
}
