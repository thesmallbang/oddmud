using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public interface IEncounter
    {

        int Id { get; }
        event Func<IEncounter, EncounterEndings, Task> Ended;
        event Func<IEncounter, ICombatAction, Task> ActionExecuted;


        List<IEntity> Dead { get; }

        Dictionary<IEntity, ICombatant> Combatants { get; }
        List<ICombatAction> ActionLog { get; }

        Task TickAsync(IGame game);

        Task TerminateAsync();

    }
}