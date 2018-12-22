using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public interface IEncounter
    {

        int Id { get; }
        event Func<IEncounter, EncounterEndings, Task> Ended;

        Dictionary<IEntity, ICombatant> Combatants { get; }

        Task TickAsync(IGame game);

        Task TerminateAsync();

    }
}