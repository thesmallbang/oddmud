using System;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface ISpawnable
    {
        event Func<ISpawnable, IMap, Task> Spawned;

        Task SpawnAsync(IMap map);
        

    }
}