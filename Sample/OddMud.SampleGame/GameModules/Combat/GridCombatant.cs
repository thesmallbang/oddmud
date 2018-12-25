using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridCombatant : ICombatant<GridTargetAction>, IEntityComponent
    {

        public GridEntity TargetPreference { get; set; }


        public bool CanAttack
        {
            get
            {
                return DateTime.Now.AddMilliseconds(-attackDelay) > LastAction;
            }
        }
        public DateTime LastAction = DateTime.Now;
        private int attackDelay => 2000;
        private Random _random = new Random();


        // these will override the intelligence with manually queued commands
        public Queue<GridTargetAction> Actions { get; } = new Queue<GridTargetAction>();

        public IEncounterIntelligence Intelligence { get; set; } 

        public List<IStat> Stats => _stats;

        public List<GridTargetAction> AllowedActions { get; set; }

        private List<IStat> _stats = new List<IStat>();

        public virtual async Task<GridTargetAction> GetNextActionAsync(IEncounter encounter)
        {
            GridTargetAction action = null;

            if (CanAttack)
            {
                action = Actions.Count > 0 ? Actions.Dequeue() :(GridTargetAction)await Intelligence.GetNextActionAsync(encounter);

                if (TargetPreference != null && encounter.Dead.Contains(TargetPreference))
                    TargetPreference = null;


                if (TargetPreference != null && (action.TargetType == TargetTypes.Enemy
                     && !action.TargetEntities.Any()))
                {
                    action.TargetEntities.Add(TargetPreference);
                }


                LastAction = DateTime.Now;
            }

            return action;

        }

    }
}
