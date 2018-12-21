using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public abstract class BasicEntity : IEntity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IMap Map { get; set; }

        public event Func<IItem, IEntity, Task> ItemPickedUp;
        public event Func<IItem, IEntity, Task> ItemDropped;

        public virtual IReadOnlyList<IItem> Items => _items;



        private List<IItem> _items = new List<IItem>();

        public BasicEntity(int id, string name, IEnumerable<IItem> items)
        {
            Id = id;
            Name = name;
            _items.AddRange(items);
        }



        public virtual async Task PickupItemAsync(IItem item)
        {

            await Map.RemoveItemAsync(item);
            _items.Add(item);
            await item.MarkAsPickedUpAsync(this);

            if (ItemPickedUp != null)
                await ItemPickedUp.Invoke(item, this);

        }

        public virtual async Task DropItemAsync(IItem item)
        {
        
            await Map.AddItemAsync(item);
            _items.Remove(item);
            await item.MarkAsDroppedAsync(this);

            if (ItemDropped != null)
                await ItemDropped.Invoke(item, this);
        }

        public virtual Task<ISpawnable> SpawnAsync(IGame game)
        {
            throw new Exception("Spawn not implemented");
        }
    }
}
