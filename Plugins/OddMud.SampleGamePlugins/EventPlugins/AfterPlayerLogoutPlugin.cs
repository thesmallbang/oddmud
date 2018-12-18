using OddMud.BasicGame;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerLogoutPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerLogoutPlugin);
        public IGame Game;

        public void Configure(IGame game, IServiceProvider serviceProvider)
        {
            Game = game;
            Game.PlayerRemoved += AfterPlayerLogout;
        }

        private async Task AfterPlayerLogout(Object sender, IPlayer player)
        {

            await player.Map.RemovePlayerAsync(player);

            var playersLeftBehind = player.Map.Players;
            var leftBehindNotification = new MudLikeCommandBuilder().AddPlayers(playersLeftBehind).Build(ViewCommandType.Replace, "playerlist");

            await Game.Network.SendViewCommandsToMapAsync(player.Map, leftBehindNotification);
            
        }

    }
}
