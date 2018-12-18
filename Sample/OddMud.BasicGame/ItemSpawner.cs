using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class ItemSpawner : ISpawner
    {
        public virtual SpawnType SpawnType { get; set; } = SpawnType.Item;

        public int ItemId { get; set; }

        // the time from the item pickup until the next spawn
        public int ResetDuration { get; set; } = 10 * 1000;
        private DateTime _lastReset;
        
        public ISpawnable SpawnedEntity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual Task SpawnerTick()
        {
            return Task.CompletedTask;
        }

        public virtual Task ResetAsync()
        {
            
            _lastReset = DateTime.Now;
            return Task.CompletedTask;
        }

        public ItemSpawner(int itemId)
        {
            ItemId = itemId;
        }
    }
}
