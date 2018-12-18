using Microsoft.Extensions.Logging;
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


        public GridGame(
            ILogger<GridGame> logger,
            ITransport network,
            IWorld world,
            IStorage storage) : base(logger, network, world, storage)
        {



        }
    }
}
