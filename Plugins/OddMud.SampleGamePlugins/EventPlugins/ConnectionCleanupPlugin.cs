using OddMud.BasicGame;
using OddMud.BasicGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class ConnectionCleanupPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(ConnectionCleanupPlugin);
        public override int Interval => 5000;


        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            game.Network.Disconnected += Network_Disconnected;
        }
        
        public override async Task IntervalTick(object sender, EventArgs e)
        {
            await base.IntervalTick(sender, e);
            foreach (var player in Game.Players)
            {
                if (!Game.Network.Connections.Contains(player.TransportId))
                    await Game.RemovePlayerAsync(player);
            }
        }

        private Task Network_Disconnected(object sender, string transportId)
        {
            Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "NetworkDisconnected" + transportId);
            return Game.RemovePlayerAsync(Game.Players.GetPlayerByTransportId(transportId));
        }

    }
}
