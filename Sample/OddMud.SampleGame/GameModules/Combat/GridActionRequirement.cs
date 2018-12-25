using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridActionReqirement : IActionRequirement
    {

        public string Name { get; set; }
        public int Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
