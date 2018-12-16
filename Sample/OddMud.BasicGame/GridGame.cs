using OddMud.BasicGame.Misc;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame
{
    public class GridGame : Game
    {

        public new GridWorld World { get { return (GridWorld)base.World; } }


        public GridGame(Microsoft.Extensions.Logging.ILogger<GridGame> logger, Core.Interfaces.ITransport network, IWorld world) : base(logger, network, (IWorld)world)
        {

            World.AddMap(new GridMap("mapkey0", "Sample Map", "Some description of the map", new GridLocation(), new List<GridExits>() { GridExits.East, GridExits.Up }));
            World.AddMap(new GridMap("mapkey1", "Other Map", "Some description of the other map", new GridLocation(1,0), new List<GridExits>() { GridExits.West }));
        }
    }
}
