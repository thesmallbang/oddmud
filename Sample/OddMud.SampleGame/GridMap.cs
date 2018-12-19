using OddMud.SampleGame.Misc;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{

    public class GridMap : BasicMap
    {

        public IReadOnlyList<Exits> Exits => _exits;
        private List<Exits> _exits = new List<Exits>();
        public GridLocation Location = new GridLocation();


        public GridMap()
        {

        }

        public GridMap(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public GridMap(int id, string name, string description, GridLocation worldLocation, IEnumerable<Exits> exits) : this(id, name, description, worldLocation)
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
        public void AddExit(Exits direction)
        {
            if (!_exits.Contains(direction))
                _exits.Add(direction);
        }

        public void RemoveExit(Exits direction)
        {
            _exits.Remove(direction);
        }



    }
}
