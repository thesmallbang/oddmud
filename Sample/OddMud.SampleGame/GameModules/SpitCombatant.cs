using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
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


        public Queue<ICombatAction<GridEntity>> Actions { get; } = new Queue<ICombatAction<GridEntity>>();

        public ICombatAction DefaultAction => new SpitAction();

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
