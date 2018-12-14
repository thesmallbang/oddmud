using System.Collections.Generic;

namespace OddMud.Core.Plugins
{
    public interface IPluginLoader<TPlugin>
       where TPlugin : IPlugin
    {

        IReadOnlyList<TPlugin> Plugins { get; }
        void LoadPlugins(string path);
    }
}
