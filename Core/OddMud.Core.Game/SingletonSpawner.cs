using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public abstract class SingletonSpawner : ISpawner<ISpawnable>
    {

        public virtual event Func<ISpawnable, Task> Spawned;

        public virtual ISpawnable SpawnedEntity { get; private set; }
        public virtual int EntityId { get; set; }

        public virtual SpawnType SpawnType { get; set; } = SpawnType.Item;

        public int ResetDuration { get; set; } = 10 * 1000;

        private DateTime _lastReset = DateTime.MinValue;

        public virtual Task SpawnAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task SpawnerTickAsync()
        {
            if (SpawnedEntity != null)
                return Task.CompletedTask;

            if (_lastReset.AddMilliseconds(ResetDuration) < DateTime.Now)
                return Task.CompletedTask;

            return SpawnAsync();
        }

        public virtual Task Reset(ISpawnable spawnable)
        { 
            SpawnedEntity = null;
            _lastReset = DateTime.Now;
            return Task.CompletedTask;
        }
    }

}
