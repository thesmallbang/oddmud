using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerLogoutPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerLogoutPlugin);
        public BasicGame.Game Game;

        public void Configure(IGame game)
        {
            Game = (BasicGame.Game)game;
            Game.PlayerRemoved += AfterPlayerLogout;
        }

        private async Task AfterPlayerLogout(Object sender, IPlayer player)
        {

            await player.Map.RemovePlayerAsync(player);


        }

    }
}
