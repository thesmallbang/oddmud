using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins
{
    public class MockLoginPlugin : IProcessorPlugin<IProcessorData<CommandModel>>
    {
        public string Name => nameof(MockLoginPlugin);
        private Game Game;

        public void Configure(IGame game)
        {
            Game = (Game)game;
        }

        public async Task ProcessAsync(IProcessorData<CommandModel> request)
        {
            if (request.Data.Parts[0] == "login")
            {
                request.Handled = true;
                await HandleBasicLoginAsync(request);
            }

            if (request.Data.Parts[0] == "logout")
            {
                request.Handled = true;
                await HandleBasicLogoutAsync(request);
            }

        }


        private async Task HandleBasicLoginAsync(IProcessorData<CommandModel> request)
        {
            var player = Game.Players.GetPlayerByTransportId(request.TransportId);
            if (player != null)
            {
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Already logged in as {player.Name}.");
                return;
            }

            player = new BasicPlayer() { Name = request.Data.Parts[1], TransportId = request.TransportId };
            if (await Game.AddPlayerAsync(player))
            {
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, "Logged in as " + request.Data.Parts[1]);
                await Game.World.MovePlayerAsync(player, Game.World.GetStarterMap());

            }
        }

        private async Task HandleBasicLogoutAsync(IProcessorData<CommandModel> request)
        {
            var player = Game.Players.GetPlayerByTransportId(request.TransportId);
            if (player == null)
                return;

            if (await Game.RemovePlayerAsync(player))
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, "Logged out");

        }


    }
}
