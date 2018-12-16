using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OddMud.BasicGame;
using OddMud.Core.Interfaces;
using System;
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
                                         IGame game)
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
                    _logger.LogDebug($"GameService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                await Game.TickAsync();
                await Task.Delay(_settings.LoopDelay, stoppingToken);
            }

            _logger.LogDebug($"GameService background task is stopping.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Run your graceful clean-up actions
            return Task.CompletedTask;
        }
    }

}
