using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules
{
    public class SpitAction : GridAction
    {


        public override List<IActionModifier> Modifiers { get; set; } = new List<IActionModifier>() { new GridActionModifier() {Name = "health", ModifierType = ActionModifierType.Percent, Min  = -5, Max = -5 }};

        public override async Task<bool> Execute()
        {

            if (TargetEntity == null)
                return false;

            return await base.Execute();
        }


        public override void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} ")
                .AddText("spits", TextColor.Aqua)
                .AddText(" on ")
                .AddText($"{TargetEntity.Name} for ")
                .AddText($"{DamageDone}", TextColor.Red)
                .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }



    }
}
