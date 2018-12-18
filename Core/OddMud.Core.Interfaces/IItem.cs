using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IItem : ISpawnable
    {

        event Func<IItem, IEntity, Task> PickedUp;
        event Func<IItem, IEntity, Task> Dropped;

        
    }
}
