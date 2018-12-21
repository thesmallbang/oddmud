using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

            var gridPlayer = (GridPlayer)player;
            
            var assignedPlayerMap = gridPlayer.Map != null ? gridPlayer.Map :  Game.World.Maps.FirstOrDefault(m => m.Location.X == 0 && m.Location.Y == 0 && m.Location.Z == 0);
            if (assignedPlayerMap == null)
            {
                assignedPlayerMap = new GridMap(0, "Void Map", "You have entered a world with nothing. use map commands to begin");
                assignedPlayerMap  = await Game.Store.NewMapAsync(Game, assignedPlayerMap);
                await Game.World.AddMapAsync(assignedPlayerMap);
            }

            await Game.World.MovePlayerAsync(player, assignedPlayerMap);



        }

    }
}
