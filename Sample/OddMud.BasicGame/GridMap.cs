using OddMud.BasicGame.Misc;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{

    public class GridMap : IMap
    {
        public int Id { get; private set; } = 0;

        public string Name { get; set; } = "Unconfigured";

        public string Description { get; set; } = "Unconfigured";

        public IReadOnlyList<GridExits> Exits => _exits;
        private List<GridExits> _exits = new List<GridExits>();

        public IReadOnlyList<ISpawner> Spawners => _spawners;
        private List<ISpawner> _spawners = new List<ISpawner>();



        public GridLocation Location = new GridLocation();

        public IReadOnlyList<IPlayer> Players => _players;

        public IReadOnlyList<IItem> Items => _items;
        private List<IItem> _items = new List<IItem>();
        public IReadOnlyList<IEntity> Npcs => _npcs;
        private List<IEntity> _npcs { get; set; }


        private List<IPlayer> _players = new List<IPlayer>();

        public GridMap()
        {

        }

        public GridMap(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public GridMap(int id, string name, string description, GridLocation worldLocation, IEnumerable<GridExits> exits) : this(id, name, description, worldLocation)
        {
            _exits = exits.ToList();
        }

        public GridMap(int id, string name, string description, GridLocation worldLocation) : this(id, name, description)
        {
            Location = worldLocation;
        }


        /**
         *  Add events for all these list modifier methods
         * 
         */


        /// <summary>
        /// You should probably be using World.MovePlayer in most scenarios to make sure all the correct events are triggered
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task AddPlayerAsync(IPlayer player)
        {
            player.Map = this;
            _players.Add(player);
            return Task.CompletedTask;
        }
        public virtual Task RemovePlayerAsync(IPlayer player)
        {
            _players.Remove(player);
            return Task.CompletedTask;
        }

        public void AddExit(GridExits direction)
        {
            if (!_exits.Contains(direction))
                _exits.Add(direction);
        }

        public void RemoveExit(GridExits direction)
        {
            _exits.Remove(direction);
        }

        public void AddSpawner(ISpawner spawner)
        {
            _spawners.Add(spawner);
        }
        public void RemoveSpawner(ISpawner spawner)
        {
            _spawners.Remove(spawner);
        }

        public void AddItem(IItem item)
        {
            _items.Add(item);
        }
        public void RemoveItem(IItem item)
        {
            _items.Remove(item);
        }
        

        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }
}
