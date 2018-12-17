using OddMud.Core.Interfaces;
using System;

namespace OddMud.Core.Plugins
{
    public interface IEventPlugin : IPlugin
    {
        void Configure(IGame game, IServiceProvider serviceProvider);

    }
}
