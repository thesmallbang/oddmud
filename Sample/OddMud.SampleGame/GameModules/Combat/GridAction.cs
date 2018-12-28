using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;


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
        public virtual List<IActionRequirement> Requirements { get; set; } = new List<IActionRequirement>();



        public virtual async Task<bool> Execute()
        {
            if (!SourceEntity.IsAlive)
                return false;

            if (!SourceEntity.CurrentStatsAtLeast(Modifiers.Where(m => m.TargetType == ModifierTargetTypes.Caster)))
                return false;



            return await Executing();
        }

        public virtual Task<bool> Executing()
        {
            ExecutedTime = DateTime.Now;

            return Task.FromResult(true);
        }

        public virtual Task SetDefaultTargetAsync(IEncounter encounter)
        {

            return Task.CompletedTask;

        }

    

    }
}
