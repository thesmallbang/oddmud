using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class Game : IGame
    {
        private readonly ILogger<Game> _logger;
        public string Name = nameof(Game);
        public ITransport Network { get; }
        public IWorld World { get; }

        public IReadOnlyList<IPlayer> Players { get { return _players; } }
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public event Func<object, EventArgs, Task> Ticked;


        public Game(
            ILogger<Game> logger,
            ITransport network,
            IWorld world
            )
        {
            _logger = logger;
            Network = network;
            World = world;
            _logger.LogDebug($"IGame Injection");
        }

        public virtual bool AddPlayer(IPlayer player)
        {
            if (Players.Any(p => p.Name == player.Name))
            {
                Network.SendMessageToPlayer(player.TransportId, "User is already logged in.");
                return false;
            }
            _players.Add(player);
            return true;
        }



        public virtual Task TickAsync()
        {
            Ticked?.Invoke(this, null);
            return Task.FromResult(0);
        }

        public virtual void Log(LogLevel level, string message)
        {
            _logger?.Log(level, message);
        }


    }
}
