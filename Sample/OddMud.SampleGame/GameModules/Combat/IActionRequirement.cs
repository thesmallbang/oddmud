using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public interface IActionRequirement
    {
        int Id { get; set; }
        string Name { get; set; }
        int Min { get; set; }
        int Max { get; set; }

    }
}
