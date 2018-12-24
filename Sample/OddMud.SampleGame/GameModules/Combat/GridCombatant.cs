using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridCombatant : ICombatant<ICombatAction<GridEntity>>, IEntityComponent
    {

        public bool CanAttack
        {
            get
            {
                return DateTime.Now.AddMilliseconds(-attackDelay) > LastAction;
            }
        }
        public DateTime LastAction = DateTime.Now;
        private int attackDelay => 2000;
        private Random _random = new Random();

        public Queue<ICombatAction<GridEntity>> Actions { get; } = new Queue<ICombatAction<GridEntity>>();

        public IEncounterIntelligence Intelligence { get; set; } 

        //public ICombatAction DefaultAction
        //{
        //    get
        //    {
        //        var rnd = _random.Next(1, 10);
        //        if (rnd < 2)
        //        {
        //            var natureElement = new Element() { Name = "Nature",  TextColor = View.MudLike.TextColor.Purple, Ranges = new List<IElementRange>() { new ElementRange() {Min = 0, Max = 10, Text = "barely heals", TextColor = View.MudLike.TextColor.Green }, new ElementRange() {Min = 11,  Text = "heals", TextColor = View.MudLike.TextColor.Green } } };
        //            return new GridTargetAction() { Id = 0, Name = "Heal Group", Element = natureElement, TargetType = TargetTypes.FriendArea, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = 5, Max = 20, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };
        //        }
        //        else
        //        {
        //            var salivaElement = new Element() { Name = "Saliva", TextColor = View.MudLike.TextColor.Aqua, Ranges = new List<IElementRange>() { new ElementRange() { Text = "hits", TextColor = View.MudLike.TextColor.Red } } };
        //            return new GridTargetAction() { Id = 0, Name = "Spit", Element = salivaElement , TargetType = TargetTypes.Enemy, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = -10, Max = -5, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };
        //        }
        //    }
        //}

        public List<IStat> Stats => _stats;
        private List<IStat> _stats = new List<IStat>();

        public virtual async Task<ICombatAction<GridEntity>> GetNextActionAsync(IEncounter encounter)
        {
            ICombatAction<GridEntity> action = null;

            if (CanAttack)
            {
                action = Actions.Count > 0 ? Actions.Dequeue() :(ICombatAction<GridEntity>)await Intelligence.GetNextActionAsync(encounter);
                LastAction = DateTime.Now;
            }

            return action;

        }

    }
}
