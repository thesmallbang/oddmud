using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OddMud.BasicGame;
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

        public BasicGame.Game Game;


        public GameService(IOptions<GameServiceSettings> settings,
                                         ILogger<GameService> logger,
                                         BasicGame.Game game)
        {
            _logger = logger;
            _logger.LogDebug($"Ctor {nameof(GameService)}");
            _settings = settings.Value;

            Game = game;
            game.Name = "Got it coach.";
            
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GameService is starting.");

            stoppingToken.Register(() =>
                    _logger.LogDebug($" GameService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                await Game.TickAsync();
                await Task.Delay(_settings.LoopDelay, stoppingToken);
            }

            _logger.LogDebug($"GameService background task is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            // Run your graceful clean-up actions
            await Task.FromResult(0);
        }
    }

}
