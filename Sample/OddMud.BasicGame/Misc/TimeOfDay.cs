using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Misc
{
    public class TimeOfDay
    {
        public double Timescale = 1;

        // in game time to begin from when the game starts
        public long StartOffset = DateTime.Now.Ticks;

        private long _created = DateTime.Now.Ticks;



        public DateTime WorldTime
        {
            get
            {
                var elapsed = DateTime.Now.Ticks - _created;
                var ticks =  StartOffset + (Timescale * elapsed);
                return new DateTime(Convert.ToInt64(ticks));
            }
        }
        public DateTime ServerTime => DateTime.Now;

    }
}
