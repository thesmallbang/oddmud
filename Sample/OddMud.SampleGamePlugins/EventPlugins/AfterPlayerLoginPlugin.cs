using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using OddMud.SampleGame.ViewComponents;
using OddMud.View.ComponentBased;

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

            var playerData = new PlayerData() { Id = player.Id, Name = player.Name };



            var vitals = new List<string>() { "health", "mana", "stamina" };
            player.Stats.Where(s => vitals.Contains(s.Name)).ToList().ForEach(async vital =>
            {
                var percent = (1 / (double)vital.Base) * 100;
                await vital.ApplyAsync((int)percent);

                var currentStatPercent = Convert.ToInt32(((double)vital.Value / (double)vital.Base * 100));
                switch (vital.Name)
                {
                    case "health":
                        playerData.Health = currentStatPercent;
                        break;
                    case "mana":
                        playerData.Mana = currentStatPercent;
                        break;
                    case "stamina":
                        playerData.Stamina = currentStatPercent;
                        break;
                }

            });
            var statUpdate = ComponentViewBuilder<ComponentTypes>.Start().AddComponent(ComponentTypes.PlayerData, playerData);
            await Game.Network.SendViewCommandsToPlayerAsync(player, statUpdate);

        }

    }
}
