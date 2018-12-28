using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class Element : IElement
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<IElementRange> Ranges { get; set; }

        public ElementRange GetRange(int value)
        {
            var absValue = Math.Abs(value);
            var match = Ranges.FirstOrDefault(r => r.Min.GetValueOrDefault(int.MinValue) <= absValue
            && r.Max.GetValueOrDefault(int.MaxValue) >= absValue);

            if (match != null)
                return (ElementRange)match;

            return new ElementRange() { Min = value, Max = value, Text = "touches" };

        }

    }
}
