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
        public TimeOfDay Time = new TimeOfDay() { Timescale = 60, StartOffset = new DateTime(2000, 1, 1).Ticks };


        private List<IMap> _maps = new List<IMap>();
        private readonly ILogger<BasicWorld> _logger;
        private readonly ITransport _network;

        public IReadOnlyList<IMap> Maps => _maps;

        public event Func<object, IMapChangeEvent, Task> PlayerMoved;

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
            return _maps.First();
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
            _logger.LogInformation($"Moving {player.Name} to {map.Name}");

            var oldMap = player.Map;
            await map.AddPlayerAsync(player);
            if (player.Map == map)
            {
                await PlayerMoved(this, new PlayerMovedEventArgs(player, oldMap, map));
            }

        }
    }
}
