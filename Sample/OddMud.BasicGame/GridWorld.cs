using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OddMud.Core.Interfaces;

namespace OddMud.BasicGame
{
    public class GridWorld : BasicWorld
    {
        public new IReadOnlyList<GridMap> Maps => base.Maps.ToList().Select(m => (GridMap)m).ToList();


        public GridWorld(Microsoft.Extensions.Logging.ILogger<GridWorld> logger, Core.Interfaces.ITransport network) : base(logger, network)
        {

        }


        public override void AddMap(IMap map)
        {
            var gridMap = (GridMap)map;
            if (Maps.Any(m => m.Location.X == gridMap.Location.X && m.Location.Y == gridMap.Location.Y && m.Location.Z == gridMap.Location.Z))
                throw new Exception("duplicate map location");

            base.AddMap(map);
        }

    }
}
