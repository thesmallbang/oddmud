using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IEntity : ISpawnable
    {
        int Id { get; set; }
        string Name { get; }
        IMap Map { get; set; }

        

        event Func<IItem,IEntity, Task> ItemPickedUp;
        event Func<IItem,IEntity, Task> ItemDropped;


        Task PickupItemAsync(IItem item);
        Task DropItemAsync(IItem item);

    }
}
