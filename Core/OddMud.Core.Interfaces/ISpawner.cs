using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface ISpawner
    {

        Task SpawnerTickAsync();
    }

    public interface ISpawner<T> : ISpawner
        where T : ISpawnable
    {
        event Func<T, Task> Spawned;

    }

   
    

}
