using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface ISpawner
    {

        Task SpawnerTickAsync(IGame game);
    }

    public interface ISpawner<T, TMap> : ISpawner
        where T : ISpawnable
    {
        event Func<T,TMap, Task> Spawned;

    }

   
    

}
