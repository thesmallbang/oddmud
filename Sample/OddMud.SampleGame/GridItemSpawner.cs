using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{


    public class GridItemSpawner : GridSpawner
    {
        public override SpawnType SpawnType => SpawnType.Item;

        public GridItemSpawner(int mapId, int itemId)
        {
            MapId = mapId;
            EntityId = itemId;
        }

        public GridItemSpawner(GridMap map, int itemId)
        {
            MapId = map.Id;
            Map = map;
            EntityId = itemId;
        }

        public GridItemSpawner(GridMap map, int itemId, int respawnDelayMilliseconds)
        {
            MapId = map.Id;
            Map = map;
            EntityId = itemId;
            ResetDuration = respawnDelayMilliseconds;
        }

        public override async Task SpawnAsync(IGame game)
        {

            if (Map == null)
                throw new Exception("Map was created and the map was never set to match the MapId");

            
            var storageItem = (GridItem)game.Items.FirstOrDefault(i => i.Id == EntityId);
            var item = new GridItem(storageItem.Id,storageItem.Name,storageItem.Description,storageItem.ItemTypes.ToList(), storageItem.Stats.Select(s => (BasicStat)s).ToList());
            await game.World.AddItemAsync(item);
            await Map.AddItemAsync(item);

            SpawnedEntity = item;
            // we want to know when the item is picked up as our reset flag to know we can spawn again after the delay
            item.PickedUp += ResetSpawner;
            await base.SpawnAsync(game);
        }

        private async Task ResetSpawner(IItem item, IEntity whoPickedUp)
        {
            // we dont care about this item being picked up anymore and it would actually cause issues when the item gets pickedup/dropped again and again later
            item.PickedUp -= ResetSpawner;
           
            await base.Reset(item);
        }



    }
}
