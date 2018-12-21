using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerLoginPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerLoginPlugin);
        public GridGame Game;

        public void Configure(IGame game, IServiceProvider serviceProvider)
        {
            Game = (GridGame)game;
            Game.PlayerAdded += AfterPlayerLogin;
        }

        private async Task AfterPlayerLogin(Object sender, IPlayer player)
        {


            var assignedPlayerMap = Game.World.GetStarterMap();
            if (assignedPlayerMap == null)
            {
                assignedPlayerMap = new GridMap(0, "Void Map", "You have entered a world with nothing. use map commands to begin");
                var map = await Game.Store.NewMapAsync(Game, assignedPlayerMap);
                await Game.World.AddMapAsync(map);
            }

            await Game.World.MovePlayerAsync(player, assignedPlayerMap);



        }

    }
}
