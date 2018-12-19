using Microsoft.Extensions.Logging;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame
{
    public class GridGame : BasicGame
    {

        public new GridWorld World => (GridWorld)base.World;


        public GridGame(
            ILogger<GridGame> logger,
            ITransport network,
            IWorld world,
            IStorage storage) : base(logger, network, world, storage)
        {



        }
    }
}
