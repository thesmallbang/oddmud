using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{
    public class CombatModule : IGameModule<CombatModule>
    {
        private readonly ILogger<CombatModule> _logger;
        private readonly IGame _game;
        private readonly CombatModuleSettings _settings;

        public IReadOnlyList<GridEncounter> Encounters => _encounters;
        private List<GridEncounter> _encounters = new List<GridEncounter>();

        private int _encounterCounter;


        public CombatModule(
            ILogger<CombatModule> logger,
            CombatModuleSettings settings,
            IGame game
            )
        {
            _logger = logger;
            _game = game;
            _settings = settings;
        }


        public Task TickAsync()
        {
            return Task.CompletedTask;
        }

        public Task<bool> IsPlayerInCombatAsync(IPlayer player)
        {
            var inEncounter = _encounters.Any(e => e.Combatants.ContainsKey(player));
            return Task.FromResult<bool>(inEncounter);
        }

        private Task AddEncounterAsync(GridEncounter encounter)
        {
            _encounters.Add(encounter);
            return Task.CompletedTask;
        }
        private Task RemoveEncounterAsync(GridEncounter encounter)
        {
            _encounters.Add(encounter);
            return Task.CompletedTask;
        }

        private Task MergeEncounters(GridEncounter keepEncounter, GridEncounter mergedEncounter)
        {
            return Task.CompletedTask;
        }

        public async Task<GridEncounter> AppendOrNewEncounterAsync(GridEntity initiated, GridEntity target)
        {

            GridEncounter currentEncounter = null;

            // we need to see if the 2 entities are already in other encounters.. of so we will merge.. otherwise we join or create one
            var encounterForInitated = _encounters.FirstOrDefault(e => e.Combatants.Any(c => c.Key == initiated));
            var encounterForTarget = _encounters.FirstOrDefault(e => e.Combatants.Any(c => c.Key == target));
            if (encounterForInitated != null
                && encounterForTarget != null
                && encounterForInitated != encounterForTarget)
            {
                // merge

            }
            else
            {
                currentEncounter = encounterForTarget == null ? encounterForInitated : encounterForTarget;
                if (currentEncounter == null)
                {
                    // create 
                    _encounterCounter++;
                    currentEncounter = new GridEncounter(_encounterCounter, new Dictionary<IEntity, ICombatant>());
                    currentEncounter.Ended += EncounterEnded;
                    await AddEncounterAsync(currentEncounter);
                }

                // join
                if (!currentEncounter.Combatants.ContainsKey(initiated))
                    currentEncounter.Combatants.Add(initiated, (ICombatant)initiated.EntityComponents.FirstOrDefault(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant))));

                if (!currentEncounter.Combatants.ContainsKey(target))
                    currentEncounter.Combatants.Add(target, (ICombatant)initiated.EntityComponents.FirstOrDefault(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant))));



            }

            
            return currentEncounter;


        }

        private Task EncounterEnded(IEncounter encounter, EncounterEndings endingType)
        {
            _encounters.Remove((GridEncounter)encounter);
            return Task.CompletedTask;
        }
    }
}
