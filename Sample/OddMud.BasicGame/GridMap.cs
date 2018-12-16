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
        public string Id { get; private set; } = "0";

        public string Name { get; private set; } = "Unconfigured";

        public string Description { get; private set; } = "Unconfigured";

        public IReadOnlyList<GridExits> Exits => _exits;
        private List<GridExits> _exits = new List<GridExits>();


        public GridLocation Location = new GridLocation();

        public IReadOnlyList<IPlayer> Players => _players;
        private List<IPlayer> _players = new List<IPlayer>();

        public GridMap()
        {

        }

        public GridMap(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public GridMap(string id, string name, string description, GridLocation worldLocation, IEnumerable<GridExits> exits) : this(id, name, description, worldLocation)
        {
            _exits = exits.ToList();
        }

        public GridMap(string id, string name, string description, GridLocation worldLocation) : this(id,name,description)
        {
            Location = worldLocation;
        }


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


        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }
}
