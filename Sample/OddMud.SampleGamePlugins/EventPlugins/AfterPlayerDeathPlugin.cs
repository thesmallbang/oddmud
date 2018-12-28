using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AfterPlayerDeathPlugin : IEventPlugin
    {

        public string Name => nameof(AfterPlayerDeathPlugin);
        public GridGame Game;
        private ILogger<AfterPlayerDeathPlugin> _logger;


        public void Configure(IGame game, IServiceProvider serviceProvider)
        {
            Game = (GridGame)game;
            _logger = (ILogger<AfterPlayerDeathPlugin>)serviceProvider.GetService(typeof(ILogger<AfterPlayerDeathPlugin>));
            Game.PlayerAdded += PlayerLoggedIn;
            Game.PlayerRemoved += PlayerLoggedOut;
        }

        private Task PlayerLoggedOut(object sender, IPlayer player)
        {
            player.Died -= PlayerDied;
            return Task.CompletedTask;
        }

        private async Task PlayerDied(IEntity player)
        {

            _logger.LogInformation($"Player {player.Name} Died.");
            // what happens when a player dies?

            // leave a corpse

            // damage worn items?

            // fill stats
            var vitalstats = new List<string>() { "health", "mana", "stamina" };
            player.Stats
                .Where(o => vitalstats.Contains(o.Name))
                .ToList().ForEach(s => s.Fill());

            // take experience
            var experience = player.Stats.FirstOrDefault(s => s.Name == "experience");
            if (experience != null)
            {
                var modifier = experience.Base / 10;
                await experience.ApplyAsync(-modifier);
            }

            // send to center map
            await Game.World.MovePlayerAsync((IPlayer)player, Game.World.Maps.FirstOrDefault(m => m.Location.X == 0 && m.Location.Y == 0 && m.Location.Z == 0));


        }

        private Task PlayerLoggedIn(object sender, IPlayer player)
        {
            player.Died += PlayerDied;
            return Task.CompletedTask;
        }
    }
}
