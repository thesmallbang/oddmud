using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{
    public class GridSpawner : SingletonSpawner
    {
        public int Id { get; set; }


        public int MapId { get; set; }
        public GridMap Map { get; set; }


    }


    public class GridItemSpawner : GridSpawner
    {
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

        public override async Task SpawnAsync()
        {

            if (Map == null)
                throw new Exception("Map was created and the map was never set to match the MapId");

            var item = new BasicItem();
            await Map.AddItemAsync(item);

            SpawnedEntity = item;
            // we want to know when the item is picked up as our reset flag to know we can spawn again after the delay
            item.PickedUp += ResetSpawner;
            await base.SpawnAsync();
        }

        private async Task ResetSpawner(IItem item, IEntity whoPickedUp)
        {
            // we dont care about this item being picked up anymore and it would actually cause issues when the item gets pickedup/dropped again and again later
            item.PickedUp -= ResetSpawner;
            await Map.RemoveItemAsync(item);

            await base.Reset(item);
        }



    }
}
