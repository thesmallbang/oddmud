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

        public Task<GridEncounter> FindOrNewEncounter(IEntity initiated, IEntity target, out string issue)
        {
            issue = string.Empty;

            return Task.FromResult<GridEncounter>(null);
        }

    }
}
