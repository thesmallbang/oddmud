using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface ISpawner
    {
        int Id { get; set; }
        Task SpawnerTickAsync(IGame game);
    }

    public interface ISpawner<T, TMap> : ISpawner
        where T : ISpawnable
        where TMap : IMap
    {
        event Func<T,TMap, Task> Spawned;

    }

   
    

}
