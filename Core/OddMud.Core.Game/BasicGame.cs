using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class BasicGame : IGame
    {
        private readonly ILogger<BasicGame> _logger;
        public string Name = nameof(BasicGame);
        public ITransport Network { get; }
        public IStorage Store { get; }

        public IWorld World { get; }

        public IReadOnlyList<IPlayer> Players { get { return _players; } }

        public IReadOnlyList<IItem> Items => _items;

        private readonly List<IItem> _items = new List<IItem>();
        private readonly List<IPlayer> _players = new List<IPlayer>();

        public event Func<object, EventArgs, Task> Ticked;
        public event Func<object, IPlayer, Task> PlayerAdded;
        public event Func<object, IPlayer, Task> PlayerRemoved;


        public BasicGame(
            ILogger<BasicGame> logger,
            ITransport network,
            IWorld world,
            IStorage storage
            )
        {
            _logger = logger;
            Network = network;
            Store = storage;
            World = world;
            _logger.LogDebug($"Injection : IGame");

        }

        public virtual async Task<bool> AddPlayerAsync(IPlayer player)
        {
            if (Players.Any(p => p.Name == player.Name))
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

           var removed = _players.Remove(player);

            if (removed && PlayerRemoved != null)
                await PlayerRemoved(this, player);

            return removed;
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

        public virtual Task<bool> AddItemAsync(IItem item)
        {
            _items.Add(item);
            return Task.FromResult(true);
        }

        public virtual Task<bool> RemoveItemAsync(IItem item)
        {
            return Task.FromResult(_items.Remove(item));
        }

        public async Task<bool> AddSpawnerAsync(ISpawner spawner)
        {
            await World.AddSpawnerAsync(spawner);

            return true;
        }

        public async Task<bool> RemoveSpawnerAsync(ISpawner spawner)
        {
            await World.RemoveSpawnerAsync(spawner);

            return true;
        }
    }
}
