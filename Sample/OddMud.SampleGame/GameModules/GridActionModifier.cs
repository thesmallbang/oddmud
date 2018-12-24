using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.GameModules
{
    public class GridActionModifier : IActionModifier
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public string Name { get; set; }

        public ActionModifierType ModifierType { get; set; }

        private Random _random = new Random();

        public int Value => _random.Next(Min, Max);
    }
}
