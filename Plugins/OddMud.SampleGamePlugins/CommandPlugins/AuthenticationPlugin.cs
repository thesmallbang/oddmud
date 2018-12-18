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
    public class AuthenticationPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(AuthenticationPlugin);
        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "login", "logout", "register" };


        public override async Task NotLoggedInProcessAsync(IProcessorData<CommandModel> request)
        {

            if (request.Data.FirstPart == "register")
            {
                if (string.IsNullOrEmpty(request.Data.ThirdPart))
                {
                    await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"format: register charactername password");
                    return;
                }

                var created = await Game.Store.NewPlayerAsync(new BasicPlayer() { Name = request.Data.SecondPart }, request.Data.ThirdPart);
                if (created)
                    await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Character {request.Data.SecondPart} created");
                else
                    await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Registration rejected");
                return;
            }


            if (request.Data.FirstPart != "login" || string.IsNullOrEmpty(request.Data.SecondPart))
                return;

            request.Handled = true;
            await HandleBasicLoginAsync(request);
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

            var player = (BasicPlayer)await Game.Store.LoadPlayerAsync(request.Data.SecondPart, request.Data.ThirdPart);
            if (player == null)
            {
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Login was rejected");
                return;
            }

            player.TransportId = request.TransportId;

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
