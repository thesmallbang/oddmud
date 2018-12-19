using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IGame
    {
        IWorld World { get; }
        IStorage Store { get; }
        // players that are online
        // may move this players into the world and have this players become the db of players
        // basically IGame is the db lists and IWorld is the instance lists
        IReadOnlyList<IPlayer> Players { get; }

        // database of all items in the game (not their instances)
        IReadOnlyList<IItem> Items { get; }


        ITransport Network { get; }

        event Func<object, EventArgs, Task> Ticked;
        event Func<object, IPlayer, Task> PlayerAdded;
        event Func<object, IPlayer, Task> PlayerRemoved;


        Task TickAsync();
        Task<bool> AddPlayerAsync(IPlayer player);
        Task<bool> RemovePlayerAsync(IPlayer player);

        Task<bool> AddItemAsync(IItem item);
        Task<bool> RemoveItemAsync(IItem item);
        
        Task<bool> AddSpawnerAsync(ISpawner spawner);
        Task<bool> RemoveSpawnerAsync(ISpawner spawner);


        void Log(LogLevel level, string message);
    }
}
