using OddMud.BasicGame;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerLoginPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerLoginPlugin);
        public IGame Game;

        public void Configure(IGame game)
        {
            Game = game;
            Game.PlayerAdded += AfterPlayerLogin;
        }

        private async Task AfterPlayerLogin(Object sender, IPlayer player)
        {
            await Game.World.MovePlayerAsync(player, Game.World.GetStarterMap());
        }

    }
}
