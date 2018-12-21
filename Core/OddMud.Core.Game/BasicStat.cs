using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Game
{
    public class BasicStat : IStat
    {
        public virtual string Name {get;set;}

        public virtual int Current { get; set; }

        public virtual int Base { get; set; }


        public BasicStat(string name, int statbase, int statcurrent)
        {
            Name = name;
            Base = statbase;
            Current = statcurrent;

        }

    }
}
