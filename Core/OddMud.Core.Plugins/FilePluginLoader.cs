using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OddMud.Core.Plugins
{
    public class FilePluginLoader<TPlugin> : IPluginLoader<TPlugin>
      where TPlugin : IPlugin
    {
        public IReadOnlyList<TPlugin> Plugins => _plugins;
        private List<TPlugin> _plugins = new List<TPlugin>();
        private readonly ILogger<FilePluginLoader<TPlugin>> _logger;

        public FilePluginLoader(ILogger<FilePluginLoader<TPlugin>> logger)
        {
            _logger = logger;
            _logger.LogDebug($"Ctor FilePluginLoader<{typeof(TPlugin).FullName}>");
        }

        public virtual void LoadPlugins(string path)
        {
            var files = System.IO.Directory.EnumerateFiles(path);
            foreach (var fileName in files.Where(file => file.EndsWith(".dll")))
            {
                var fileInfo = new FileInfo(fileName);
                var loader = PluginLoader.CreateFromAssemblyFile(fileInfo.FullName, sharedTypes: new[] { typeof(TPlugin) });
                var assembly = loader.LoadDefaultAssembly();

                var pluginTypes = assembly.GetTypes().ToList();

                pluginTypes.ForEach((p) =>
                {
                    p.GetInterfaces().ToList().ForEach(i =>
                    {
                        if (i == typeof(TPlugin))
                        {
                            _logger.LogDebug($"Plugin Activating : {typeof(TPlugin).FullName}");
                            var plugin = (TPlugin)Activator.CreateInstance(p);
                            _plugins.Add(plugin);
                            _logger.LogDebug($"Plugin Added : {plugin.Name}");
                        }
                    });
                });




            }
        }
    }
}
