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
        public int Minimum { get; set; }

        public virtual int Value { get; set; }

        public virtual int Base { get; set; }

        public event Func<IStat, int, Task> StatChanged;

        public BasicStat(string name, int statbase, int statcurrent)
        {
            Name = name;
            Base = statbase;
            Value = statcurrent;

        }

        public virtual async Task ApplyAsync(int modifier)
        {
            var original = Value;
            Value += modifier;

            Value = Math.Clamp(Value, Minimum, Base);

            if (StatChanged != null)
                await StatChanged.Invoke(this, original);


        }

        public virtual async Task Fill()
        {
            var original = Value;
            Value = Base;

            if (StatChanged != null)
                await StatChanged.Invoke(this, original);
        }
    }
}
