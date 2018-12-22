using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class BasicStat : IStat
    {
        public virtual string Name { get; set; }

        public virtual int Current { get; set; }

        public virtual int Base { get; set; }

        public event Func<IStat, int, Task> StatChanged;

        public BasicStat(string name, int statbase, int statcurrent)
        {
            Name = name;
            Base = statbase;
            Current = statcurrent;

        }

        public async Task ApplyToCurrentAsync(int modifier)
        {
            var original = Current;
            Current += modifier;

            if (StatChanged != null)
                await StatChanged.Invoke(this, original);


        }
    }
}
