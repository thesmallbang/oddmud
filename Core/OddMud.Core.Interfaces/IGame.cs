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
        IReadOnlyList<IPlayer> Players { get; }
                
        ITransport Network { get; }

        event Func<object, EventArgs, Task> Ticked;
        event Func<object, IPlayer, Task> PlayerAdded;
        event Func<object, IPlayer, Task> PlayerRemoved;


        Task TickAsync();
        Task<bool> AddPlayerAsync(IPlayer player);
        Task<bool> RemovePlayerAsync(IPlayer player);


        void Log(LogLevel level, string message);
    }
}
