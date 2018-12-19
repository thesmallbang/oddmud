using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class ConnectionCleanupPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(ConnectionCleanupPlugin);
        public override int Interval => 60 * 1000;


        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
            game.Network.Disconnected += Network_Disconnected;
        }
        
        public override async Task IntervalTick(object sender, EventArgs e)
        {
            await base.IntervalTick(sender, e);

            var disconnectedPlayers = Game.Players.Where(o=> !Game.Network.Connections.Contains(o.TransportId)).ToList();
            disconnectedPlayers.ForEach(async (p) => await Game.RemovePlayerAsync(p));
        }

        private Task Network_Disconnected(object sender, string transportId)
        {
            Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "NetworkDisconnected" + transportId);
            return Game.RemovePlayerAsync(Game.Players.GetPlayerByTransportId(transportId));
        }

    }
}
