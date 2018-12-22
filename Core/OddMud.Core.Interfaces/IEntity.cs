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

        IReadOnlyList<IStat> Stats { get; }


        event Func<IItem, IEntity, Task> ItemPickedUp;
        event Func<IItem, IEntity, Task> ItemDropped;
        event Func<IEntity, Task> Died;



        Task PickupItemAsync(IGame game, IItem item);
        Task DropItemAsync(IGame game, IItem item);

        Task KillAsync();

    }
}
