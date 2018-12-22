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
using Microsoft.Extensions.Logging;
using OddMud.View.MudLike;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class SpawnManagerPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(SpawnManagerPlugin);
        public override int Interval => 1000;


        private List<GridSpawner> _gridSpawners = new List<GridSpawner>();
        private DateTime _spawnersUpdated = DateTime.Now.AddSeconds(-50);
        private ILogger<SpawnManagerPlugin> _logger;

        private int cacheDuration = 60 * 1000;


        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            _logger = (ILogger<SpawnManagerPlugin>)serviceProvider.GetService(typeof(ILogger<SpawnManagerPlugin>));
            base.Configure(game, serviceProvider);
        }

        public override async Task IntervalTick(object sender, EventArgs e)
        {

            if (_spawnersUpdated.AddMilliseconds(cacheDuration) < DateTime.Now)
            {
                _logger.LogDebug("Updating spawners cache");
                _spawnersUpdated = DateTime.Now;

                var inboundSpawners = Game.World.Spawners.Select(o => (GridSpawner)o).ToList();
                var missingSpawners = _gridSpawners.Except(inboundSpawners).ToList();
                var newSpawners = inboundSpawners.Except(_gridSpawners).ToList();

                if (missingSpawners.Any())
                {
                    foreach (var missingSpawner in missingSpawners)
                    {
                        _logger.LogDebug("removing spawner spawn sub");
                        missingSpawner.Spawned -= SpawnerSpawned;
                    }
                }

                if (newSpawners.Any())
                {
                    foreach (var newSpawner in newSpawners)
                    {
                        _logger.LogDebug("adding spawner spawn sub");
                        newSpawner.Spawned += SpawnerSpawned;
                    }
                    _gridSpawners.AddRange(newSpawners);
                }



            }

            _gridSpawners.ForEach(async (spawner) =>
            {
                await spawner.SpawnerTickAsync(Game);
            });

            await base.IntervalTick(sender, e);

        }

        private Task SpawnerSpawned(ISpawnable arg, IMap map)
        {

            var itemsUpdate = MudLikeOperationBuilder.Start("itemlist").AddItems(map.Items)
           .Build();
            return Game.Network.SendViewCommandsToMapAsync(map, MudLikeViewBuilder.Start().AddOperation(itemsUpdate).Build());


        }
    }
}
