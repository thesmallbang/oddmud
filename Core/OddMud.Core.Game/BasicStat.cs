using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Game
{
    public class BasicStat : IStat
    {
        public virtual string Name => nameof(BasicStat);

        public virtual int Current => Base;

        public virtual int Base { get; }


    }
}
