using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules.Combat
{


    public interface ICombatAction
    {
        DateTime ExecutedTime { get; set; }

        Task<bool> Execute();


        List<IActionModifier> Modifiers { get; set; }


    }
    public interface ICombatAction<TEntity> : ICombatAction
        where TEntity : IEntity
    {
        TEntity SourceEntity { get; set; }


        Task SetDefaultTargetAsync(IEncounter encounter);


    }
}
