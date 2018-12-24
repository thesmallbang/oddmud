using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{

    public class ElementRange
    {
        public int? Min { get; set; }
        public int? Max { get; set; }

        public string Text { get; set; }
        public TextColor TextColor { get; set; }


    }
}
