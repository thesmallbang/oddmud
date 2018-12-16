using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Misc
{

    [Flags]
    public enum GridExits
    {
        None,
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
        Up,
        Down
    }
}
