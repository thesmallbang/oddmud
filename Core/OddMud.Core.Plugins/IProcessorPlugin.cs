using OddMud.Core.Interfaces;
using System.Threading.Tasks;

namespace OddMud.Core.Plugins
{
    public interface IProcessorPlugin<TData> : IPlugin
    {
        Task ProcessAsync(TData request);
        void Configure(IGame game);
    }
}
