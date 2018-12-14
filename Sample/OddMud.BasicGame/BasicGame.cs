using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{
    public class BasicGame : IGame
    {
        private readonly ILogger<BasicGame> _logger;
        public string Name = nameof(BasicGame);
        public ITransport Network { get; }
        public IWorld World { get; }

        public IReadOnlyList<IPlayer> Players { get { return _players; } }
        private readonly List<IPlayer> _players = new List<IPlayer>();




        public BasicGame(
            ILogger<BasicGame> logger,
            ITransport network,
            IWorld world
            )
        {
            _logger = logger;
            Network = network;
            World = world;
            _logger.LogDebug($"IGame Injection: {nameof(BasicGame)}");
        }

        public virtual bool AddPlayer(IPlayer player)
        {
            if (Players.Any(p => p.Name == player.Name))
            {
                Network.SendMessageToPlayer(player.NetworkId, "User is already logged in.");
                return false;
            }
            _players.Add(player);
            return true;
        }



        public virtual Task TickAsync()
        {
            return Task.FromResult(0);
        }

        public virtual void Log(LogLevel level, string message)
        {
            _logger?.Log(level, message);
        }


    }
}
