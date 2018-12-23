using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IMap
    {
        int Id { get; }

        string Name { get; }
        string Description { get; }

        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<IItem> Items { get; }
        IReadOnlyList<IEntity> Entities { get; }


        event Func<IMap, IReadOnlyList<IItem>, Task> ItemsChanged;
        event Func<IMap, IReadOnlyList<IPlayer>, Task> PlayersChanged;
        event Func<IMap, IReadOnlyList<IEntity>, Task> EntitiesChanged;



        Task AddPlayerAsync(IPlayer player);
        Task RemovePlayerAsync(IPlayer player);

        Task AddItemAsync(IItem item);
        Task RemoveItemAsync(IItem item);

        Task AddEntityAsync(IEntity entity);
        Task RemoveEntityAsync(IEntity entity);


    }
}
