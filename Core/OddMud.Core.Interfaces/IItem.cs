using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface IItem : ISpawnable
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }

        event Func<IItem, IEntity, Task> PickedUp;
        event Func<IItem, IEntity, Task> Dropped;

        Task MarkAsPickedUpAsync(IEntity entityWhoPickedUp);
        Task MarkAsDroppedAsync(IEntity entityWhoDropped);

        IReadOnlyList<IStat> Stats { get; }




    }
}
