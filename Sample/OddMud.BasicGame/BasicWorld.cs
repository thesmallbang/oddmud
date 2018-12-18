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
        public TimeOfDay Time = new TimeOfDay() { Timescale = 6000 };
        public virtual IReadOnlyList<IMap> Maps => _maps;

        private readonly ILogger<BasicWorld> _logger;
        private readonly ITransport _network;
        private List<IMap> _maps = new List<IMap>();

        public BasicWorld(
            ILogger<BasicWorld> logger,
            ITransport transport
            )
        {
            _logger = logger;
            _logger.LogDebug($"Injection : IWorld");
            _network = transport;
        }

        public virtual IMap GetStarterMap()
        {
            if (Maps.Count == 0)
            {
                _maps.Add(new GridMap(0, "Void", "This world has no maps yet. Start creating with map commands"));
            }

            return Maps[0];
        }

        public virtual void AddMap(IMap map)
        {

            if (Maps.Any(m => m.Id == map.Id))
            {
                throw new Exception("Duplicate map Id");
            }
          
            _maps.Add(map);
        }


        public virtual async Task MovePlayerAsync(IPlayer player, IMap map)
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
