using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{
    public interface ICombatant
    {
        ICombatAction GetDefaultAction { get; }
        bool CanAttack { get; }

        Queue<ICombatAction> Actions { get;  }

        ICombatAction GetNextAction();

        event Func<ICombatant, IEncounter, Task> Death;



    }
}
