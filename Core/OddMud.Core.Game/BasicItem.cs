using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class BasicItem : IItem
    {
        public string Name => nameof(BasicItem);

        public event Func<IItem, IEntity, Task> PickedUp;
        public event Func<IItem, IEntity, Task> Dropped;
        public event Func<ISpawnable, IMap, Task> Spawned;


        public Task MarkAsPickedUpAsync(IEntity entityWhoPickedUp)
        {
            if (PickedUp != null)
                return PickedUp.Invoke(this, entityWhoPickedUp);

            return Task.CompletedTask;
        }

        public Task MarkAsDroppedAsync(IEntity entityWhoDropped)
        {
            if (Dropped != null)
                return Dropped.Invoke(this, entityWhoDropped);

            return Task.CompletedTask;
        }

        public virtual Task SpawnAsync(IMap map)
        {


            if (Spawned != null)
                return Spawned.Invoke(this, map);

            return Task.CompletedTask;
        }
    }
}
