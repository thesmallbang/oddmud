using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class ItemSpawner : SingletonSpawner
    {
        private readonly IMap _map;
        public override event Func<ISpawnable, Task> Spawned;


        public override async Task SpawnAsync()
        {


            var item = new BasicItem();
            await _map.AddItemAsync(item);

            if (Spawned != null)
                await Spawned.Invoke(item);

            // we want to know when the item is picked up as our reset flag to know we can spawn again after the delay
            item.PickedUp += ResetSpawner;
        }

        private async Task ResetSpawner(IItem item, IEntity whoPickedUp)
        {
            // we dont care about this item being picked up anymore and it would actually cause issues when the item gets pickedup/dropped again and again later
            item.PickedUp -= ResetSpawner;
            await _map.RemoveItemAsync(item);

            await base.Reset(item);
        }


        public ItemSpawner(IMap map, int itemId)
        {
            EntityId = itemId;
            _map = map;
        }
    }
}
