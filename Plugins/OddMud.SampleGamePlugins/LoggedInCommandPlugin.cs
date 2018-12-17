using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins
{
    public abstract class LoggedInCommandPlugin : IProcessorPlugin<IProcessorData<CommandModel>>
    {
        public virtual string Name => nameof(LoggedInCommandPlugin);
        public virtual IGame Game { get; private set; }
        public virtual IReadOnlyList<string> Handles => new List<string>();


        public virtual void Configure(IGame game,
            IServiceProvider serviceProvider)
        {
            Game = game;
        }

        public async Task ProcessAsync(IProcessorData<CommandModel> request)
        {

            if (!Handles.Any(handleCommand => handleCommand == request.Data.FirstPart))
                return;
            Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, $"PlayerCount: {Game.Players.Count}");
            var player = Game.Players.GetPlayerByTransportId(request.TransportId);
            if (player == null)
            {
                Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, $"In Process {Name} - NotLoggedIn");
                await NotLoggedInProcessAsync(request);
            }
            else
            {
                Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, $"In Process {Name} - LoggedIn");
                await LoggedInProcessAsync(request, player);
            }
        }

        public virtual Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            return Task.CompletedTask;
        }

        public virtual Task NotLoggedInProcessAsync(IProcessorData<CommandModel> request)
        {
            return Task.CompletedTask;
        }
    }
}
