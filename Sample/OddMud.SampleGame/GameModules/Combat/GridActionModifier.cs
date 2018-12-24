using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridActionModifier : IActionModifier
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public string Name { get; set; }

        public ActionModifierType ModifierType { get; set; }

        private Random _random = new Random();

        private int? _value;

        public int Value
        {
            get
            {
                if (!_value.HasValue) _value = _random.Next(Min, Max);
                return _value.GetValueOrDefault(0);
            }
        }


        public ModifierTargetTypes TargetType { get; set; }
    }
}
