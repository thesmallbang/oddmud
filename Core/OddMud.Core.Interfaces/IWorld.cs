using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IWorld
    {


        event Func<object, IMapChangeEvent, Task> PlayerMoved;

        IReadOnlyList<IMap> Maps { get; }
        IReadOnlyList<ISpawner> Spawners { get; }


        string Name { get; }

        Task MovePlayerAsync(IPlayer player, IMap map);

        Task AddMapAsync(IMap map);
        Task RemoveMapAsync(IMap map);

        Task AddSpawnerAsync(ISpawner spawner);
        Task RemoveSpawnerAsync(ISpawner spawner);

        IMap GetStarterMap();




    }
}
