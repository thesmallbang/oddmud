using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{


    public class GridEntitySpawner : GridSpawner
    {


        public GridEntitySpawner(int mapId, int itemId)
        {
            MapId = mapId;
            EntityId = itemId;
        }

        public GridEntitySpawner(GridMap map, int itemId)
        {
            MapId = map.Id;
            Map = map;
            EntityId = itemId;
        }

        public GridEntitySpawner(GridMap map, int itemId, int respawnDelayMilliseconds)
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


            var storageNpc = (GridEntity)game.World.Entities.FirstOrDefault(i => i.Id == EntityId);
            var entity = new GridEntity(storageNpc.Id, storageNpc.Name, (EntityClasses)storageNpc.Class, storageNpc.EntityTypes, storageNpc.EntityComponents, storageNpc.Items);
            entity.Map = Map;
            await game.World.AddEntityAsync(entity);
            await Map.AddEntityAsync(entity);

            SpawnedEntity = entity;
            // we want to know when the item is picked up as our reset flag to know we can spawn again after the delay
            entity.Died += ResetSpawner;
            await base.SpawnAsync(game);
        }

        private async Task ResetSpawner(IEntity whoDied)
        {
            // we dont care about this item being picked up anymore and it would actually cause issues when the item gets pickedup/dropped again and again later
            whoDied.Died -= ResetSpawner;

            await Map.RemoveEntityAsync(whoDied);

            await base.Reset(whoDied);
        }



    }
}
