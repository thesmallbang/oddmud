using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IWorld
    {


        event Func<object, IMapChangeEvent, Task> PlayerMoved;

        IEnumerable<IMap> Maps { get; }
        IEnumerable<IItem> Items { get; }
        IEnumerable<ISpawner> Spawners { get; }
        IEnumerable<IEntity> Entities { get; }

        string Name { get; }

        Task MovePlayerAsync(IPlayer player, IMap map);

        Task AddMapAsync(IMap map);
        Task RemoveMapAsync(IMap map);
        Task AddItemAsync(IItem item);
        Task RemoveItemAsync(IItem item);


        Task AddSpawnerAsync(ISpawner spawner);
        Task RemoveSpawnerAsync(ISpawner spawner);
        Task AddEntityAsync(IEntity entity);
        Task RemoveEntityAsync(IEntity entity);


        IMap GetStarterMap();




    }
}
