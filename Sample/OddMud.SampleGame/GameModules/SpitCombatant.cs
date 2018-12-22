using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;

namespace OddMud.SampleGame.GameModules
{
    public class SpitCombatant : ICombatant, IEntityComponent
    {
        public ICombatAction GetDefaultAction => throw new NotImplementedException();

        public bool CanAttack => throw new NotImplementedException();

        public Queue<ICombatAction> Actions => throw new NotImplementedException();


        public event Func<IEncounter, Task> Death;

        public ICombatAction GetNextAction()
        {
            throw new NotImplementedException();
        }
    }
}
