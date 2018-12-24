using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class SpitAction : GridSingleTargetAction
    {


        public override List<IActionModifier> Modifiers { get; set; } = new List<IActionModifier>() { new GridActionModifier() {Name = "health", ModifierType = ActionModifierType.Percent, Min  = -5, Max = -5 }};

     

     



    }
}
