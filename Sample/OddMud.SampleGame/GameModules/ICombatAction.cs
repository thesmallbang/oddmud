using OddMud.Core.Interfaces;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{


    public interface ICombatAction
    {
        void AppendToOperation(IOperationBuilder builder);
        string ToMessage();

    }
    public interface ICombatAction<TEntity> : ICombatAction
        where TEntity : IEntity
    {
        TEntity SourceEntity { get; set; }

        // not all actions will need a target
        TEntity TargetEntity { get; set; }

        Task Execute();

        Task SetDefaultTargetAsync(IEnumerable<TEntity> entities);


    }
}
