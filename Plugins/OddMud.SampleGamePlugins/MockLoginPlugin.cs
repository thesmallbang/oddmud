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
        }
 

        private async Task HandleBasicLoginAsync(IProcessorData<CommandModel> request)
        {
            var player = Game.Players.GetPlayerByNetworkId(request.TransportId);
            if (player != null)
            {
                Game.Network.SendMessageToPlayer(request.TransportId, $"Already logged in as {player.Name}.");
                return;
            }

            player = new BasicPlayer() { Name = request.Data.Parts[1], TransportId = request.TransportId };
            if (Game.AddPlayer(player))
            {
                Game.Network.SendMessageToPlayer(request.TransportId, "Logged in as " + request.Data.Parts[1]);
                await Game.World.MovePlayerAsync(player, Game.World.GetStarterMap());

            }
        }

      
    }
}
