using System;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface ISpawnable
    {
        event Func<IMap, Task> Spawned;

        Task SpawnAsync(IMap map, ISpawner spawner = null);
        

    }
}