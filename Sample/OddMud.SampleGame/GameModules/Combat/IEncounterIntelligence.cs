using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat
{
    public interface IEncounterIntelligence
    {
        EntityClasses Class { get;  }

        Task<ICombatAction> GetNextActionAsync(IEncounter encounter);

    }
}