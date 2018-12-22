using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;

namespace OddMud.SampleGame.GameModules
{
    public class SpitCombatant : ICombatant<ICombatAction<GridEntity>>, IEntityComponent
    {

        public bool CanAttack => true;

        public Queue<ICombatAction<GridEntity>> Actions => throw new NotImplementedException();

        public ICombatAction DefaultAction => new SpitAction();

        public Task<ICombatAction<GridEntity>> GetNextActionAsync()
        {

            return Task.FromResult<ICombatAction<GridEntity>>(null);

        }
    }
}
