using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class AutoSavingPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(AutoSavingPlugin);
        public new GridGame Game => (GridGame)base.Game;

        public override int Interval => 5 * 60 * 1000;
        private ILogger<AutoSavingPlugin> _logger;

        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
            _logger = (ILogger<AutoSavingPlugin>)serviceProvider.GetService(typeof(ILogger<AutoSavingPlugin>));
        }

        public override async Task IntervalTick(object sender, EventArgs e)
        {
            _logger.LogInformation($"Saving...");
             var stopwatch = new Stopwatch();
            stopwatch.Start();            
            await Game.Store.UpdatePlayersAsync(Game, Game.Players);
            await Game.Store.UpdateItemsAsync(Game, Game.Items);
            await Game.Store.UpdateMapsAsync(Game, Game.World.Maps);

            stopwatch.Stop();
            
            _logger.LogInformation($"Saved. Took {stopwatch.ElapsedMilliseconds} mmilliseconds");
            await base.IntervalTick(sender, e);

        }

        public override Task IntervalSkipped(object sender, EventArgs e)
        {
            return base.IntervalSkipped(sender, e);
        }

    }
}
