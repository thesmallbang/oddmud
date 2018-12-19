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
    public class SpawnManagerPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(SpawnManagerPlugin);
        public override int Interval => 5000;


        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
        }
        
        public override async Task IntervalTick(object sender, EventArgs e)
        {
            await base.IntervalTick(sender, e);

            Game.World.Spawners.ToList().ForEach(async (spawner) => {
                await spawner.SpawnerTickAsync();
            });

        }



    }
}
