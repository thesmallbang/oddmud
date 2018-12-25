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
    public class GridAction : ICombatAction<GridEntity>
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public TargetTypes TargetType { get; set; }

        public GridEntity SourceEntity { get; set; }

        public Element Element { get; set; }

    
        public DateTime ExecutedTime { get; set; }


        public virtual List<IActionModifier> Modifiers { get; set; } = new List<IActionModifier>();


        public virtual async Task<bool> Execute()
        {
            if (!SourceEntity.IsAlive)
                return false;


            ExecutedTime = DateTime.Now;

            return await Executing();
        }

        public virtual Task<bool> Executing()
        {
            return Task.FromResult(true);
        }

        public virtual Task SetDefaultTargetAsync(IEncounter encounter)
        {

            return Task.CompletedTask;

        }

        public virtual void AppendToOperation(IOperationBuilder operationBuilder)
        {

        }

        public virtual string ToMessage()
        {
            return "";
        }


    }
}
