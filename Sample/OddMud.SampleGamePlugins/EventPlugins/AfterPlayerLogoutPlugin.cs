using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
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
            await Game.Store.UpdatePlayersAsync(Game, new List<IPlayer>() { player });

            var playersLeftBehind = player.Map.Players;
            var leftBehindNotification = MudLikeOperationBuilder.Start("playerlist").AddPlayers(playersLeftBehind).Build();

            await Game.Network.SendViewCommandsToMapAsync(player.Map, MudLikeViewBuilder.Start().AddOperation(leftBehindNotification).Build());

        }

    }
}
