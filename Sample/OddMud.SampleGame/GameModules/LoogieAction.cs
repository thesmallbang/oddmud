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
    public class LoogieAction : GridAction
    {

        private Random _randomizer = new Random();



        public override async Task<bool> Execute()
        {


            if (TargetEntity == null)
                return false;

            if (SourceEntity.Map != TargetEntity.Map)
                return false;

            var dmg = _randomizer.Next(10, 20);
            Damage = dmg;

            var hpstat = TargetEntity.Stats.FirstOrDefault(s => s.Name == "health");
            if (hpstat == null)
                return false;

            await hpstat.ApplyAsync(-dmg);

            if (hpstat.Value == 0)
            {
                await TargetEntity.KillAsync();
            }

            return await base.Execute();
        }

        

        public override void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} ")
                .AddText(" hocks a loogie ", TextColor.Lime)
                .AddText(" on ")
                .AddText($"{TargetEntity.Name} for ")
                .AddText($"{Damage}", TextColor.Red)
                .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

      

    }
}
