using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{

    public class DamageRangeInfo
    {
        public int? Min { get; set; }
        public int? Max { get; set; }

        public string Description { get; set; }
        public TextColor TextColor { get; set; }


    }
}
