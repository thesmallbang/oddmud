using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Misc
{
    public class TimeOfDay
    {
        public double timeScale = 1;
        public long StartWorldTime = DateTime.Now.Ticks;
        private long Birth = DateTime.Now.Ticks;



        public DateTime WorldTime
        {
            get
            {
                var elapsed = Math.Abs(DateTime.Now.Ticks - Birth);
                var ticks =  StartWorldTime + (timeScale * elapsed);
                return new DateTime(Convert.ToInt64(ticks));
            }
        }
        public DateTime ServerTime => DateTime.Now;

    }
}
