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
    public class LoogieAction : GridSingleTargetAction
    {

        public override List<IActionModifier> Modifiers { get; set; } = new List<IActionModifier>() { new GridActionModifier() {
            Name = "health", TargetType = TargetTypes.Caster,  ModifierType = ActionModifierType.Flat, Min = 0, Max = 20 }
        };


       
        

        public override void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} ")
                .AddText(" hocks a loogie ", TextColor.Lime)
                .AddText(" on ")
                .AddText($"{TargetEntity.Name} ")
          //      .AddText($"{DamageDone}", TextColor.Green)
          //      .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

      

    }
}
