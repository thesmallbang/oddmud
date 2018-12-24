using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class Element
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public TextColor TextColor { get; set; }

        public List<ElementRange> Ranges { get; set; }

        public ElementRange GetRange(int value)
        {
            var match = Ranges.FirstOrDefault(r => r.Min.GetValueOrDefault(int.MinValue) <= value
            && r.Max.GetValueOrDefault(int.MaxValue) >= value);

            if (match != null)
                return match;

            return new ElementRange() { Min = value, Max = value, Text = "touches", TextColor = TextColor.Normal };

        }

    }
}
