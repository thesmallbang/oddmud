using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Misc
{
    public class MapExit
    {
        public Direction Direction { get; set; } = Direction.None;
    }

    public enum Direction
    {
        None = 0,
        North = 1,
        NorthEast = 2,
        East = 3,
        SouthEast = 4,
        South = 5,
        SouthWest = 6,
        West = 7,
        NorthWest = 8,
        Up = 9,
        Down = 10
    }
}
