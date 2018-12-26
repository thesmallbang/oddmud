using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{


    public class GridEntitySpawner : GridSpawner
    {

        public override SpawnType SpawnType => SpawnType.Entity;
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

            // make sure to create new instances of classes instead of taking references from the storage entity

            var storageNpc = (GridEntity)game.World.Entities.FirstOrDefault(i => i.Id == EntityId);
            var entity = new GridEntity(storageNpc.Id, storageNpc.Name, storageNpc.EntityTypes, storageNpc.EntityComponents, storageNpc.Items,
                storageNpc.Stats.Select(s => new BasicStat(s.Name, s.Base, s.Value)).ToList()
                );

            // configure the intel component again
            if (entity.IsAttackable())
            {
                var component = (GridCombatant)entity.EntityComponents.First(r => r.GetType().GetInterfaces().Contains(typeof(ICombatant)));
                component.Intelligence.Configure(entity);
                
            }

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
            whoDied.Died -= ResetSpawner;
            await Map.RemoveEntityAsync(whoDied);
            await base.Reset(whoDied);
        }



    }
}
