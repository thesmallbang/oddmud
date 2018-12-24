using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class DamageType
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public List<DamageRangeInfo> Ranges { get; set; }

        public DamageRangeInfo GetRange(int value)
        {
            var match = Ranges.FirstOrDefault(r => r.Min.GetValueOrDefault(int.MinValue) >= value
            && r.Max.GetValueOrDefault(int.MaxValue) <= value);

            if (match != null)
                return match;

            return new DamageRangeInfo() { Min = value, Max = value, Description = "hits", TextColor = TextColor.Normal };

        }

    }
}
