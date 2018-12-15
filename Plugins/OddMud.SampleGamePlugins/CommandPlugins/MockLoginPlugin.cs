using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGamePlugins;
using System;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins.CommandPlugins
{
    public class MockLoginPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(MockLoginPlugin);

     
        public override Task NotLoggedInProcessAsync(IProcessorData<CommandModel> request)
        {
            request.Handled = true;
            return HandleBasicLoginAsync(request);
        }


        private async Task HandleBasicLoginAsync(IProcessorData<CommandModel> request)
        {
            if (request.Data.FirstPart != "login" || request.Data.Parts.Count < 2)
                return;

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
