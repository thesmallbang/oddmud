using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public abstract class SingletonSpawner : ISpawner<ISpawnable, IMap>
    {

        public int MapId { get; set; }
        public BasicMap Map { get; set; }

        public virtual event Func<ISpawnable,IMap, Task> Spawned;

        public virtual ISpawnable SpawnedEntity { get; set; }
        public int EntityId { get; set; }

        public virtual SpawnType SpawnType { get; set; }

        public int ResetDuration { get; set; } = 10 * 1000;
        public int Id { get; set; }

        private DateTime _lastReset = DateTime.MinValue;

        public virtual async Task SpawnAsync(IGame game)
        {

            if (Spawned != null)
                await Spawned.Invoke(SpawnedEntity, Map);

            
        }

        public virtual Task SpawnerTickAsync(IGame game)
        {
            if (SpawnedEntity != null)
                return Task.CompletedTask;

            if (_lastReset.AddMilliseconds(ResetDuration) > DateTime.Now)
                return Task.CompletedTask;

            return SpawnAsync(game);
        }

        public virtual Task Reset(ISpawnable spawnable)
        { 
            SpawnedEntity = null;
            _lastReset = DateTime.Now;
            return Task.CompletedTask;
        }
    }

}
