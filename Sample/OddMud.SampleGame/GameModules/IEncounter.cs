using System;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{
    public interface IEncounter
    {

        event Func<IEncounter, EncounterEndings,  Task> Ended;

        Task TickAsync();

        Task TerminateAsync();

    }
}