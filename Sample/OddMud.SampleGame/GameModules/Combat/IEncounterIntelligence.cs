using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat
{
    public interface IEncounterIntelligence
    {

        Task<ICombatAction> GetNextActionAsync(IEncounter encounter);


    }
}