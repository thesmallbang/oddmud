using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IGame
    {
        IWorld World { get; }
        ITransport Network { get; }

        event Func<object, EventArgs, Task> Ticked;


        Task TickAsync();

        void Log(LogLevel level, string message);
    }
}
