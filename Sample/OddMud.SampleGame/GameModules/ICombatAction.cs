using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{


    public interface ICombatAction
    {

    }
    public interface ICombatAction<TEntity> : ICombatAction
        where TEntity : IEntity
    {
        TEntity SourceEntity { get; }

        // not all actions will need a target
        TEntity TargetEntity { get; }

        Task Execute();


    }
}
