using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat.Intelligence
{
    public class GenericClassIntelligence : IEncounterIntelligence
    {


        private GridTargetAction _defaultAction;
        private List<GridTargetAction> _actions { get; set; }
        public GenericClassIntelligence(List<ICombatAction> actions, ICombatAction defaultAction)
        {
            _actions = actions.Select(a => (GridTargetAction)a).ToList();
            _defaultAction = (GridTargetAction)defaultAction;
        }


        public Task<ICombatAction> GetNextActionAsync(IEncounter encounter)
        {
            // make a copy of the action default action
            var action = new GridTargetAction() { Element = _defaultAction.Element, Modifiers = _defaultAction.Modifiers.Select(m => (IActionModifier)new GridActionModifier() { Name = m.Name, TargetType = m.TargetType, ModifierType = m.ModifierType, Min = m.Min, Max = m.Max }).ToList(), Id = _defaultAction.Id, Name = _defaultAction.Name, TargetType = _defaultAction.TargetType };


            return Task.FromResult((ICombatAction)action);
        }
    }
}
