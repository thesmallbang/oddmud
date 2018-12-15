using Microsoft.Extensions.Logging;
using OddMud.BasicGame.Events;
using OddMud.BasicGame.Misc;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class BasicWorld : IWorld
    {
        public string Name => nameof(BasicWorld);
        public event Func<object, IMapChangeEvent, Task> PlayerMoved;
        public TimeOfDay Time = new TimeOfDay() { Timescale = 60, StartOffset = new DateTime(2000, 1, 1).Ticks };
        public IReadOnlyList<IMap> Maps => _maps;

        private readonly ILogger<BasicWorld> _logger;
        private readonly ITransport _network;
        private List<IMap> _maps = new List<IMap>();

        public BasicWorld(
            ILogger<BasicWorld> logger,
            ITransport network
            )
        {
            _logger = logger;
            _logger.LogDebug($"IWorld Injection");
            _network = network;
        }

        public IMap GetStarterMap()
        {
            if (!Maps.Any())
                throw new Exception("Unable to provide starter map when none exist");

            return Maps.First();
        }

        public void AddMap(IMap map)
        {
            if (Maps.Any(m => m.Id == map.Id))
            {
                throw new Exception("Duplicate map Id");
            }
            _maps.Add(map);
        }


        public async Task MovePlayerAsync(IPlayer player, IMap map)
        {
            _logger.LogDebug($"Moving {player.Name} to {map.Name}");

            var oldMap = player.Map;
            await map.AddPlayerAsync(player);
            if (player.Map == map)
            {
                await PlayerMoved(this, new PlayerMovedEventArgs(player, oldMap, map));
            }

        }
    }
}
