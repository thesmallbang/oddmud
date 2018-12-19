using Microsoft.Extensions.Logging;
using OddMud.Core.Game.Events;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class BasicWorld : IWorld
    {
        public string Name => nameof(BasicWorld);
        public event Func<object, IMapChangeEvent, Task> PlayerMoved;
        public TimeOfDay Time = new TimeOfDay() { Timescale = 6000 };
        public virtual IEnumerable<IMap> Maps => _maps;
        private List<IMap> _maps = new List<IMap>();

        public IEnumerable<ISpawner> Spawners => _spawners;
        private List<ISpawner> _spawners = new List<ISpawner>();

        private readonly ILogger<BasicWorld> _logger;
        private readonly ITransport _network;
        
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
            if (Maps.Count() == 0)
            {
                return null;

            }

            return Maps.First();
        }

        public virtual Task AddMapAsync(IMap map)
        {

            if (Maps.Any(m => m.Id == map.Id))
            {
                throw new Exception("Duplicate map Id");
            }
          
            _maps.Add(map);
            return Task.CompletedTask;
        }

        public virtual Task RemoveMapAsync(IMap map)
        {

            if (!Maps.Any(m => m == map))
            {
                return Task.CompletedTask;
            }

            _maps.Remove(map);
            return Task.CompletedTask;
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

        public virtual Task AddSpawnerAsync(ISpawner spawner)
        {
            _spawners.Add(spawner);
            return Task.CompletedTask;
        }

        public virtual Task RemoveSpawnerAsync(ISpawner spawner)
        {
            _spawners.Remove(spawner);
            return Task.CompletedTask;
        }
    }
}
