using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IGame
    {
        IWorld World { get; }
        ITransport Network { get; }

        Task TickAsync();

        void Log(LogLevel level, string message);
    }
}
