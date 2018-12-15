using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins.EventPlugins
{
    public class OnPlayerMovePlugin : IEventPlugin
    {

        public string Name => nameof(OnPlayerMovePlugin);
        public BasicGame.Game Game;

        public void Configure(IGame game)
        {
            Game = (BasicGame.Game)game;
            Game.World.PlayerMoved += HandleMapChanged;
        }

        private async Task HandleMapChanged(Object sender, IMapChangeEvent e)
        {

            if (e.OldMap != null)
            {
                await Game.Network.RemovePlayerFromMapGroupAsync(e.Player, e.NewMap);
                await Game.Network.SendMessageToMapAsync(e.OldMap, $"{e.Player.Name} has left the area.");
            }

            // display the map information to the player
            await Game.Network.SendViewCommandsToPlayerAsync(e.Player, MudViewCommandBuilder.BuildMap(e.NewMap));

            await Game.Network.AddPlayerToMapGroupAsync(e.Player, e.NewMap);
            await Game.Network.SendMessageToMapExceptAsync(e.NewMap, e.Player, $"{e.Player.Name} has joined the area.");
            

        }

    }
}
