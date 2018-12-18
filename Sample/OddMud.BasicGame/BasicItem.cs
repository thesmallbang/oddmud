using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class BasicItem : IItem
    {
        public string Name => nameof(BasicItem);


        public event Func<IItem, IEntity, Task> PickedUp;
        public event Func<IItem, IEntity, Task> Dropped;
        public event Func<IMap, Task> Spawned;

        public ISpawner Spawner;



        public virtual Task SpawnAsync(IMap map, ISpawner spawner = null)
        {



            if (spawner != null)
                spawner.SpawnedEntity = this;


            if (Spawned != null)
                return Spawned.Invoke(map);

            return Task.CompletedTask;
        }
    }
}
