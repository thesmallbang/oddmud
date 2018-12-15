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
        public TimeOfDay Time = new TimeOfDay() {timeScale= 60,  StartWorldTime = new DateTime(2000,1,1).Ticks};


        private List<IMap> _maps = new List<IMap>();
        private readonly ILogger<BasicWorld> _logger;
        private readonly ITransport _network;

        public IReadOnlyList<IMap> Maps => _maps;

        public event Func<object, IMapChangeEvent, Task> MapChanged;

        public BasicWorld(
            ILogger<BasicWorld> logger,
            ITransport network
            )
        {
            _logger = logger;
            _logger.LogDebug($"IWorld Injection");
            _network = network;
            _maps.Add(new BasicMap("1", "TesTMap", "Some starter map"));

        }

        public IMap GetStarterMap()
        {
            return _maps.First();
        }

        public async Task MovePlayerAsync(IPlayer player, IMap map)
        {
            var oldMap = player.Map;

            _logger.LogInformation($"Moving {player.Name} to {map.Name}");
            map.AddPlayer(player);
            await MapChanged(this, new MapChangedEventArgs(player, oldMap, map));

        }
    }
}
