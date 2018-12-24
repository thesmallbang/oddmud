using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class SpitCombatant : ICombatant<ICombatAction<GridEntity>>, IEntityComponent
    {

        public bool CanAttack
        {
            get
            {

                return DateTime.Now.AddMilliseconds(-attackDelay) > _lastAction;
            }
        }
        private DateTime _lastAction = DateTime.Now;
        private int attackDelay => 2000;
        private Random _random = new Random();

        public Queue<ICombatAction<GridEntity>> Actions { get; } = new Queue<ICombatAction<GridEntity>>();

        public ICombatAction DefaultAction
        {
            get
            {
                var rnd = _random.Next(1, 10);
                if (rnd < 2)
                {
                    var natureElement = new Element() { Name = "Nature",  TextColor = View.MudLike.TextColor.Purple, Ranges = new List<ElementRange>() { new ElementRange() {Min = 0, Max = 10, Text = "barely heals", TextColor = View.MudLike.TextColor.Green }, new ElementRange() {Min = 11,  Text = "heals", TextColor = View.MudLike.TextColor.Green } } };
                    return new GridTargetAction() { Id = 0, Name = "Heal Group", Element = natureElement, TargetType = TargetTypes.FriendArea, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = 5, Max = 20, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };
                }
                else
                {
                    var salivaElement = new Element() { Name = "Saliva", TextColor = View.MudLike.TextColor.Aqua, Ranges = new List<ElementRange>() { new ElementRange() { Text = "hits", TextColor = View.MudLike.TextColor.Red } } };
                    return new GridTargetAction() { Id = 0, Name = "Spit", Element = salivaElement , TargetType = TargetTypes.Enemy, Modifiers = new List<GridActionModifier>() { new GridActionModifier() { Name = "health", Min = -10, Max = -5, ModifierType = ActionModifierType.Flat, TargetType = ModifierTargetTypes.Other } }.Select(a => (IActionModifier)a).ToList() };
                }
            }
        }

        public List<IStat> Stats => _stats;
        private List<IStat> _stats = new List<IStat>();

        public Task<ICombatAction<GridEntity>> GetNextActionAsync()
        {
            ICombatAction<GridEntity> action = null;

            if (CanAttack)
            {
                action = Actions.Count > 0 ? Actions.Dequeue() : (ICombatAction<GridEntity>)DefaultAction;
                _lastAction = DateTime.Now;
            }

            return Task.FromResult(action);

        }

    }
}
