﻿using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{
    public class CombatModule : IGameModule<CombatModule>
    {
        private readonly ILogger<CombatModule> _logger;
        private readonly IGame _game;
        private readonly CombatModuleSettings _settings;

        public IReadOnlyList<IEncounter> Encounters => _encounters;
        private List<IEncounter> _encounters = new List<IEncounter>();

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
            return Task.FromResult<bool>(false);
        }

        private Task AddEncounterAsync(IEncounter encounter)
        {
            _encounters.Add(encounter);
            return Task.CompletedTask;
        }
        private Task RemoveEncounterAsync(IEncounter encounter)
        {
            _encounters.Add(encounter);
            return Task.CompletedTask;
        }


    }
}