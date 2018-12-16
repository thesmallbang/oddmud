using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OddMud.Core.Plugins
{
    public interface IProcessorPlugin<TData> : IPlugin
    {

        IReadOnlyList<string> Handles { get; }

        Task ProcessAsync(TData request);
        void Configure(IGame game);
    }
}
