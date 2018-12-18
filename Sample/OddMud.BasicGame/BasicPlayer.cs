using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class BasicPlayer : IPlayer
    {
        public string Name { get; set; }

        public string TransportId { get; set; }

        public virtual IMap Map { get; set; }

        public virtual IReadOnlyList<IItem> Items => _items;
        private List<IItem> _items = new List<IItem>();

        public event Func<IItem, IEntity, Task> ItemPickedUp;
        public event Func<IItem, IEntity, Task> ItemDropped;
        public event Func<IMap, Task> Spawned;

        public virtual async Task PickupItemAsync(IItem item)
        {

            var map = (GridMap)Map;
            map.RemoveItem(item);
            _items.Add(item);

            if (ItemPickedUp != null)
                await ItemPickedUp.Invoke(item, this);

        }

        public virtual async Task DropItemAsync(IItem item)
        {

            var map = (GridMap)Map;
            map.AddItem(item);
            _items.Remove(item);

            if (ItemDropped != null)
                await ItemDropped.Invoke(item, this);
        }

        public virtual Task SpawnAsync(IMap map, ISpawner spawner = null)
        {

            if (Spawned != null)
                return Spawned(map);

            return Task.CompletedTask;
        }
    }
}
