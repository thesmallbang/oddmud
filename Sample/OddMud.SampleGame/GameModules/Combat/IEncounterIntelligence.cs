using OddMud.Core.Interfaces;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat
{
    public interface IEncounterIntelligence
    {

        Task<ICombatAction> GetNextActionAsync(IEncounter encounter);

        void Configure(IEntity entity);

    }
}