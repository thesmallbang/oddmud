using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class CombatModule : IGameModule<CombatModule>
    {
        private readonly ILogger<CombatModule> _logger;
        private readonly IGame _game;
        private readonly CombatModuleSettings _settings;

        public event Func<IEncounter, Task> AddedEncounter;
        public event Func<IEncounter, Task> RemovedEncounter;

        public IReadOnlyList<GridEncounter> Encounters => _encounters;
        private List<GridEncounter> _encounters = new List<GridEncounter>();

        private int _encounterCounter;
        private bool _ticking;

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


        public async Task TickAsync()
        {
            if (_ticking)
                return;

            _ticking = true;

            // cleanup expired encounters
            Encounters.Where(e => e.LastAction < DateTime.Now.AddMinutes(-1)).ToList().ForEach((e) => e.TerminateAsync(EncounterEndings.Expired));

            foreach (var encounter in _encounters.ToList())
            {
                await encounter.TickAsync(_game);
            }

            _ticking = false;
        }

        public Task<bool> IsInCombat(IEntity entity)
        {
            var inEncounter = _encounters.Any(e => e.Combatants.ContainsKey(entity));
            return Task.FromResult(inEncounter);
        }

        private Task AddEncounterAsync(GridEncounter encounter)
        {
            _encounters.Add(encounter);
            if (AddedEncounter != null)
                return AddedEncounter.Invoke(encounter);

            return Task.CompletedTask;
        }
        private Task RemoveEncounterAsync(GridEncounter encounter)
        {
            _encounters.Remove(encounter);
            if (RemovedEncounter != null)
                return RemovedEncounter.Invoke(encounter);

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
            var attackerCombatant = (GridCombatant)initiated.EntityComponents.FirstOrDefault(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant)));
            var targetCombatant = (GridCombatant)target.EntityComponents.FirstOrDefault(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant)));


            if (encounterForInitated != null
                && encounterForTarget != null
                && encounterForInitated != encounterForTarget)
            {
                // merge

            }
            else
            {

                // the same encounter already? just switch to target it
                if (encounterForInitated != null && encounterForTarget != null & encounterForInitated == encounterForTarget)
                {
                    attackerCombatant.TargetPreference = target;
                    return null;
                }

                currentEncounter = encounterForTarget == null ? encounterForInitated : encounterForTarget;
                if (currentEncounter == null)
                {
                    // create 
                    _encounterCounter++;
                    currentEncounter = new GridEncounter(_encounterCounter);
                    attackerCombatant.TargetPreference = target;
                    targetCombatant.TargetPreference = initiated;

                    await currentEncounter.AddCombatantAsync(initiated, attackerCombatant);
                    await currentEncounter.AddCombatantAsync(target, targetCombatant);

                    currentEncounter.Ended += EncounterEnded;
                    await AddEncounterAsync(currentEncounter);
                }

                // join

                if (initiated.IsAlive)
                    currentEncounter.Dead.Remove(initiated);

                if (target.IsAlive)
                    currentEncounter.Dead.Remove(target);

                if (!currentEncounter.Combatants.ContainsKey(initiated))
                {
                    attackerCombatant.TargetPreference = target;
                    await currentEncounter.AddCombatantAsync(initiated, attackerCombatant);
                }

                if (!currentEncounter.Combatants.ContainsKey(target))
                    await currentEncounter.AddCombatantAsync(target, targetCombatant);

            }


            return currentEncounter;


        }

        private Task EncounterEnded(IEncounter encounter, EncounterEndings endingType)
        {
            _logger.LogInformation("Encounter ended");
            return RemoveEncounterAsync((GridEncounter)encounter);
        }
    }
}
