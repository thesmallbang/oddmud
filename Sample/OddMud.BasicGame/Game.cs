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
        public event Func<object, IPlayer, Task> PlayerAdded;
        public event Func<object, IPlayer, Task> PlayerRemoved;


        public Game(
            ILogger<Game> logger,
            ITransport network,
            IWorld world
            )
        {
            _logger = logger;
            Network = network;
            World = world;
            _logger.LogDebug($"Injection : IGame");


        }

        public virtual async Task<bool> AddPlayerAsync(IPlayer player)
        {
            if (Players.Any(p => p == player))
                return false;

            _players.Add(player);
            if (PlayerAdded != null)
                await PlayerAdded(this, player);

            return true;
        }

        public virtual async Task<bool> RemovePlayerAsync(IPlayer player)
        {

            if (!Players.Any(p => p == player))
                return false;

            _players.Remove(player);

            if (PlayerRemoved != null)
                await PlayerRemoved(this, player);

            return true;
        }


        public virtual Task TickAsync()
        {
            Ticked?.Invoke(this, null);
            return Task.CompletedTask;
        }

        public virtual void Log(LogLevel level, string message)
        {
            _logger?.Log(level, message);
        }


    }
}
