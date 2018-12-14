using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins
{
    public class SayChatPlugin : IProcessorPlugin<IProcessorData<CommandModel>>
    {
        public string Name => nameof(SayChatPlugin);
        private Game Game;

        public void Configure(IGame game)
        {
            Game = (Game)game;
        }

        public async Task ProcessAsync(IProcessorData<CommandModel> request)
        {

            if (request.Data.Parts[0] == "say")
            {
                request.Handled = true;

                var player = Game.Players.GetPlayerByNetworkId(request.TransportId);
                if (player == null)
                {
                    Game.Network.SendMessageToPlayer(request.TransportId, $"Not logged in.");
                    return;
                }


                Game.Network.SendMessageToMap(player.Map, $"{player.Name} says {request.Data.StringFrom(1)}");
            }


        }
    }
}
