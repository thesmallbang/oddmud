using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.SampleGamePlugins;

namespace OddMud.BasicGamePlugins.CommandPlugins
{
    public class ChatPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(ChatPlugin);

        public override Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            switch (request.Data.FirstPart)
            {
                case "say":
                    return ProcessSayAsync(request, player);
                case "tell":
                    return ProcessTellAsync(request, player);

            }

            return base.LoggedInProcessAsync(request, player);
        }


        private Task ProcessSayAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;
            return Game.Network.SendMessageToMapAsync(player.Map, $"{player.Name} says {request.Data.StringFrom(1)}");
        }

        private async Task ProcessTellAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;
            var destinationPlayer = Game.Players.GetPlayerByName(request.Data.SecondPart);
            if (destinationPlayer == null)
            {
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"Player not found.");
                return;
            }

            if (destinationPlayer == player)
            {
                await Game.Network.SendMessageToPlayerAsync(request.TransportId, $"You tell yourself something really stupid.");
                return;
            }

            var message = request.Data.StringFrom(2);
            await Game.Network.SendMessageToPlayerAsync(player, $"You tell {destinationPlayer.Name}, {message}");
            await Game.Network.SendMessageToPlayerAsync(destinationPlayer, $"{player.Name} tells you, {message}");
        }


    }
}
