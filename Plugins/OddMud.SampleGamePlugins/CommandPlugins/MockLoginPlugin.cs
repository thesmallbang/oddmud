using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGamePlugins;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins.CommandPlugins
{
    public class MockLoginPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(MockLoginPlugin);
        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "login", "logout" };


        public override Task NotLoggedInProcessAsync(IProcessorData<CommandModel> request)
        {
            if (request.Data.FirstPart != "login" || string.IsNullOrEmpty(request.Data.SecondPart))
                return Task.CompletedTask;

            request.Handled = true;
            return HandleBasicLoginAsync(request);
        }

        public override Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            switch (request.Data.FirstPart)
            {
                case "login":
                    return Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Already logged in as {player.Name}.");
                case "logout":
                    return HandleBasicLogoutAsync(request, player);
            }
            return base.LoggedInProcessAsync(request, player);
        }

        private async Task HandleBasicLoginAsync(IProcessorData<CommandModel> request)
        {
            var player = new BasicPlayer() { Name = request.Data.SecondPart, TransportId = request.TransportId };

            if (await Game.AddPlayerAsync(player))
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Logged in as {request.Data.SecondPart} -- PlayerCount: {Game.Players.Count}");
            else
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Login was rejected");



        }

        private async Task HandleBasicLogoutAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            if (await Game.RemovePlayerAsync(player))
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, "Logged out");
        }


    }
}
