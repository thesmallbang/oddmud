using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat.Intelligence
{
    public class PlayerIntelligence : IEncounterIntelligence
    {

        private readonly GridTargetAction _defaultAction;


        public PlayerIntelligence(GridTargetAction defaultAction)
        {
            _defaultAction = defaultAction;
        }


        public Task<ICombatAction> GetNextActionAsync(IEncounter encounter)
        {

            var defaultAttack = new GridTargetAction() { Id = _defaultAction.Id, Name = _defaultAction.Name, Element = _defaultAction.Element, TargetType = _defaultAction.TargetType, Modifiers = _defaultAction.Modifiers.Select(m => (IActionModifier)new GridActionModifier() { Name = m.Name, TargetType = m.TargetType, ModifierType = m.ModifierType, Min = m.Min, Max = m.Max }).ToList() };
            // no intelligence right now..just set a command to find any enemy and dmg the health

            // mod this with stats eventually

            return Task.FromResult((ICombatAction)defaultAttack);
        }


    }
}
