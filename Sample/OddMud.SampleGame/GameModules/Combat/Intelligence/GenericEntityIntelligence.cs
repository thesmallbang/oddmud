using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat.Intelligence
{
    public class GenericEntityIntelligence : IEncounterIntelligence
    {


        private GridTargetAction _defaultAction;
        private List<GridTargetAction> _actions { get; set; }
        private GridEntity Entity { get; set; }

        private List<GridTargetAction> _actionsDmg = new List<GridTargetAction>();
        private List<GridTargetAction> _actionsHeal = new List<GridTargetAction>();


        public GenericEntityIntelligence(List<ICombatAction> actions, ICombatAction defaultAction)
        {
            _actions = actions.Select(a => (GridTargetAction)a).ToList();
            _defaultAction = (GridTargetAction)defaultAction;
        }

        public void Configure(IEntity entity)
        {
            Entity = (GridEntity)entity;



            // drop abilities we can't use because of stat requirements or stats consumed on using the ability our side of range
            // ie requirement level 14, strength 25   , consumer = 25 mana
            _actions.ToList().ForEach((action) =>
            {
                action.Requirements.ForEach((requirement) =>
                {
                    if (!Entity.Stats.Any(s => s.Name == requirement.Name && s.Base >= requirement.Min && s.Base <= requirement.Max))
                    {
                        _actions.Remove(action);
                    }
                });

                action.Modifiers.Where(m => m.TargetType == ModifierTargetTypes.Caster).ToList().ForEach((consumer) =>
                  {
                      if (!Entity.BaseStatAtLeast(consumer))
                      {
                          _actions.Remove(action);
                      }
                  });
            });

            // cache/categorize some generic spell types
            _actionsDmg = _actions.Where(a => a.Modifiers.Any(m => m.TargetType == ModifierTargetTypes.Other && m.Name == "health" && m.Min < 0)).ToList();
            _actionsHeal = _actions.Where(a => (a.TargetType == TargetTypes.Self || a.TargetType == TargetTypes.Friend || a.TargetType == TargetTypes.FriendArea)
            && a.Modifiers.Any(m => m.Name == "health" && m.TargetType == ModifierTargetTypes.Other && m.Min > 0)).ToList();




        }


        public Task<ICombatAction> GetNextActionAsync(IEncounter encounter)
        {

            GridTargetAction selectedAction = null;


            // what do we want to know in our generic intel?


            // randomly cast a buff? keeping it simple for now just healing and dmg

            // healing 

            var hpstat = Entity.Stats.FirstOrDefault(s => s.Name == "health");
            if (hpstat != null)
            {
                if (hpstat.Value < hpstat.Base / 2)
                {
                    var potentialActions = _actionsHeal.Where(action => Entity.CurrentStatsAtLeast(action.Modifiers.Where(m => m.TargetType == ModifierTargetTypes.Caster))).ToList();
                                       

                    if (potentialActions.Count() > 0)
                    {

                        // how much hp do we need? find the closest heal ...add some factoring of heal to mana cost effeciency later

                        var hpNeeded = hpstat.Base - hpstat.Value;
                        var differences = potentialActions.Select(action => new { Action = action, Difference = Math.Abs(hpNeeded - action.Modifiers.FirstOrDefault(m => m.Name == "health").Value) });
                        selectedAction = differences.OrderBy(d => d.Difference).FirstOrDefault().Action;
                    }
                }
            }

            // damage
            if (selectedAction == null)
            {
                // pick the hardest hitting health stat we can afford to cast
                // AI cheat boost here by knowing the randomization result of the modifier..
                var potentialActions = _actionsDmg.OrderByDescending(action => Math.Abs(action.Modifiers.FirstOrDefault(m => m.Name == "health").Value));
                foreach (var action in potentialActions)
                {
                    var consumerMods = action.Modifiers.Where(m => m.TargetType == ModifierTargetTypes.Caster).ToList();
                    // can we meet all requirements?

                    var affordable = Entity.CurrentStatsAtLeast(consumerMods);
                    if (affordable)
                    {
                        selectedAction = action;
                        break;
                    }
                }

            }


            if (selectedAction == null && Entity.CurrentStatsAtLeast(_defaultAction.Modifiers.Where(m => m.TargetType == ModifierTargetTypes.Caster)))
                selectedAction = _defaultAction;

            if (selectedAction == null)
                return null;

            var actionCopy = new GridTargetAction() { Element = selectedAction.Element, Modifiers = selectedAction.Modifiers.Select(m => (IActionModifier)new GridActionModifier() { Name = m.Name, TargetType = m.TargetType, ModifierType = m.ModifierType, Min = m.Min, Max = m.Max }).ToList(), Id = selectedAction.Id, Name = selectedAction.Name, TargetType = selectedAction.TargetType };



            return Task.FromResult((ICombatAction)actionCopy);
        }
    }
}
