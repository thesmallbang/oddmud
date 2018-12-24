using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat.Intelligence
{
    public class KnightIntelligence : IEncounterIntelligence
    {


        private static Element physical = new Element() { Name = "Physical", TextColor = View.MudLike.TextColor.Normal, Ranges = new List<IElementRange>() { new ElementRange() { Min = 0, Max = 10, Text = "hits", TextColor = View.MudLike.TextColor.Red }, new ElementRange() { Min = 11, Text = "smashes", TextColor = View.MudLike.TextColor.Red } } };
        
        public EntityClasses Class => EntityClasses.Knight;

        public Task<ICombatAction> GetNextActionAsync(IEncounter encounter)
        {

            var defaultAttack = new GridTargetAction() { Id = 0, Name = "knightly attack", Element = KnightIntelligence.physical, TargetType = TargetTypes.Enemy, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = -5, Max = 0, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };
            // no intelligence right now..just set a command to find any enemy and dmg the health
            

            return Task.FromResult((ICombatAction)defaultAttack);
        }
    }
}
