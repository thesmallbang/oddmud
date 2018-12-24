using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridPlayerCombatant : GridCombatant
    {

        private Element physical = new Element() { Name = "Physical", TextColor = View.MudLike.TextColor.Normal, Ranges = new List<IElementRange>() { new ElementRange() { Min = 0, Max = 10, Text = "hits", TextColor = View.MudLike.TextColor.Red }, new ElementRange() { Min = 11, Text = "smashes", TextColor = View.MudLike.TextColor.Red } } };

        private ICombatAction GetPlayerDefaultAction()
        {
            // auto attack.. use items sometime...
            return new GridTargetAction() { Id = 0, Name = "basic attack", Element = physical, TargetType = TargetTypes.Enemy, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = -5, Max = 0, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };

        }

        public override Task<ICombatAction<GridEntity>> GetNextActionAsync(IEncounter encounter)
        {
            ICombatAction<GridEntity> action = null;

            if (CanAttack)
            {
                action = Actions.Count > 0 ? Actions.Dequeue() : (ICombatAction<GridEntity>)GetPlayerDefaultAction();
                LastAction = DateTime.Now;
            }

            return Task.FromResult(action);

        }

    }
}
