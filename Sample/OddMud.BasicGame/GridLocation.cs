using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame
{
    public class GridLocation
    {
        public int X { get; } 
        public int Y { get; } 
        public int Z { get; } 


        public GridLocation()
        {
         
        }

        public GridLocation(int x,int y)
        {
            X = x;
            Y = y;
        }

        public GridLocation(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X}-{Y}-{Z}";
        }

      
    }
}
