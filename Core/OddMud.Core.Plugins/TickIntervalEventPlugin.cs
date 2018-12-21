using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;

namespace OddMud.Core.Plugins
{
    public abstract class TickIntervalEventPlugin : IEventPlugin
    {
        public virtual string Name => nameof(TickIntervalEventPlugin);
        public virtual IGame Game { get; private set; }

        // minumum milliseconds elapsed before firing intervaltick()..it may be longer between ticks depending on system configurations and performance etc.
        public virtual int Interval => 0;
        private long lastTick = 0;
        private bool _workingTask;


        public virtual void Configure(IGame game, IServiceProvider serviceProvider)
        {
            Game = game;
            Game.Ticked += Game_Ticked;
        }

        private Task Game_Ticked(object sender, EventArgs e)
        {
            if (!_workingTask && DateTime.Now.AddMilliseconds(-Interval).Ticks >= lastTick)
            {
                _workingTask = true;
                return IntervalTick(sender, e);
            }
            return IntervalSkipped(sender, e);
        }

        public virtual Task IntervalTick(object sender, EventArgs e)
        {
            lastTick = DateTime.Now.Ticks;
            _workingTask = false;
            return Task.CompletedTask;
        }

        public virtual Task IntervalSkipped(object sender, EventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
