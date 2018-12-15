using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins
{
    public abstract class LoggedInCommandPlugin : IProcessorPlugin<IProcessorData<CommandModel>>
    {
        public virtual string Name => nameof(LoggedInCommandPlugin);
        public BasicGame.Game Game { get; private set; }

        public void Configure(IGame game)
        {
            Game = (Game)game;
        }

        public async Task ProcessAsync(IProcessorData<CommandModel> request)
        {
            var player = Game.Players.GetPlayerByTransportId(request.TransportId);
            if (player == null)
                await NotLoggedInProcessAsync(request);
            else
                await LoggedInProcessAsync(request, player);
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
