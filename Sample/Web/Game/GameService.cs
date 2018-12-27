using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OddMud.SampleGame;
using OddMud.Core.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OddMud.Web.Game
{
    public class GameService
        : BackgroundService
    {
        private readonly ILogger<GameService> _logger;
        private readonly GameServiceSettings _settings;

        public GridGame Game;


        public GameService(IOptions<GameServiceSettings> settings,
                                         ILogger<GameService> logger,
                                         IGame game,
                                         GameHubProcessor gameHubProcessor)
        {
            _logger = logger;
            _logger.LogDebug($"ctor {nameof(GameService)}");
            _settings = settings.Value;

            Game = (GridGame)game;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GameService is starting.");

            stoppingToken.Register(() =>
                    // IGame.ShutdownAsync
                    _logger.LogDebug($"GameService background task is stopping."));

            _logger.LogInformation("Loading maps from storage");
            var maps = await Game.Store.LoadMapsAsync(Game);
            maps.ToList().ForEach(async (m) => await Game.World.AddMapAsync(m));

            _logger.LogInformation("Loading items from storage");
            var items = await Game.Store.LoadItemsAsync(Game);
            items.ToList().ForEach(async (i) => await Game.AddItemAsync(i));

            _logger.LogInformation("Loading entities from storage");
            var entities = await Game.Store.LoadEntitiesAsync(Game);
            entities.ToList().ForEach(async (i) => await Game.AddEntityAsync(i));

            _logger.LogInformation("Loading spawners from storage");
            var spawners = await Game.Store.LoadSpawnersAsync(Game);
            spawners.Select(o => (GridSpawner)o).ToList().ForEach(async (i) => {
                await Game.AddSpawnerAsync((ISpawner)i);
                await Game.World.AddSpawnerAsync((ISpawner)i);

                // set the map object on each spawner loaded from db
                i.Map = Game.World.Maps.FirstOrDefault(m => m.Id == i.MapId);
            });



            // should probably move this stuff into a IGame.StartupAsync

            while (!stoppingToken.IsCancellationRequested)
            {
                await Game.TickAsync();
                await Task.Delay(_settings.LoopDelay, stoppingToken);
            }

            _logger.LogDebug($"GameService background task is stopping.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // IGame.ShutdownAsync

            // Run your graceful clean-up actions
            return Task.CompletedTask;
        }

    }

}
