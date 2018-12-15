using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OddMud.Web.Game
{
    // may portentially move all this plugin loading somewhere else...just seems out of place here now
    public class GameHubProcessor
    {
        private readonly IReadOnlyList<IProcessorPlugin<IProcessorData<CommandModel>>> _commandPlugins;


        private readonly ILogger<GameHubProcessor> _logger;
        private readonly BasicGame.Game _game;

        public GameHubProcessor(
            ILogger<GameHubProcessor> logger,
            FilePluginLoader<IProcessorPlugin<IProcessorData<CommandModel>>> commandPluginLoader,
            FilePluginLoader<IEventPlugin> eventLoader,
            BasicGame.Game game
            )
        {
            _logger = logger;
            _game = game;

            eventLoader.LoadPlugins("./plugins/");
            var _eventPlugins = eventLoader.Plugins.ToList();
            _eventPlugins.ForEach((eplugin) => {
                _logger.LogDebug($"Configure Event Plugin {eplugin.Name}");
                eplugin.Configure(_game);
            });

            commandPluginLoader.LoadPlugins("./plugins/");
            _commandPlugins = commandPluginLoader.Plugins;
            _commandPlugins.ToList().ForEach((plugin) =>
            {
                _logger.LogDebug($"Configure Command Plugin {plugin.Name}");
                plugin.Configure(game);
            });

        }

        public async Task ProcessAsync(IProcessorData<CommandModel> request)
        {
            foreach (var plugin in _commandPlugins)
            {
                _logger.LogInformation("ProcessAsync - " + plugin.Name);
                await plugin.ProcessAsync(request);

                if (request.Handled)
                    break;

            }
        }

    }
}
