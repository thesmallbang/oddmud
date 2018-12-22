
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public class GridEncounter : IEncounter
    {
        public Dictionary<IEntity, ICombatant> Combatants { get; } = new Dictionary<IEntity, ICombatant>();


        public event Func<IEncounter, EncounterEndings, Task> Ended;

        public Task TerminateAsync()
        {
            throw new NotImplementedException();
        }

        public Task TickAsync()
        {
            throw new NotImplementedException();
        }
    }
}
