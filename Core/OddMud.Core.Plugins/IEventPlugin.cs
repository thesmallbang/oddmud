using OddMud.Core.Interfaces;

namespace OddMud.Core.Plugins
{
    public interface IEventPlugin : IPlugin
    {
        void Configure(IGame game);

    }
}
