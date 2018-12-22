
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public class GridEncounter : IEncounter
    {
        public Dictionary<IEntity, ICombatant> Combatants { get; } = new Dictionary<IEntity, ICombatant>();

        public int Id { get; }
        private int DeadCounter { get; set; }

        public event Func<IEncounter, EncounterEndings, Task> Ended;


        public GridEncounter(int id, Dictionary<IEntity,ICombatant> combatants)
        {
            Id = id;
            Combatants = combatants;

            combatants.Values.ToList().ForEach((combatant) => {
                combatant.Death += Combatant_Death;
            });

        }

        private async Task Combatant_Death(ICombatant deadCombatant, IEncounter encounter)
        {
            deadCombatant.Death -= Combatant_Death;

            if (Ended != null)
                await Ended.Invoke(this, EncounterEndings.Death);

            //var aliveNonPlayers = Combatants.Count(c => typeof(GridPlayer) != c.Key.GetType());
            //if (deadNonPlayers == 0)

    
        }

        public Task TerminateAsync()
        {
            if (Ended != null)
                Ended.Invoke(this, EncounterEndings.Other);

            return Task.CompletedTask;
        }

        public Task TickAsync()
        {
            return Task.CompletedTask;
        }
    }
}
