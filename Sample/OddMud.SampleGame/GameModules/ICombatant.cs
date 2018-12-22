﻿using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame.GameModules
{
    public interface ICombatant
    {
        ICombatAction DefaultAction { get; }
        bool CanAttack { get; }

        List<IStat> Stats { get; }

    }

    public interface ICombatant<TAction> : ICombatant
        where TAction : ICombatAction
    {

        Queue<TAction> Actions { get; }

        Task<TAction> GetNextActionAsync();
    }
}
