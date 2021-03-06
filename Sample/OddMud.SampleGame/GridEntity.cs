﻿using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OddMud.SampleGame
{
    public class GridEntity : BasicEntity
    {


        public GridEntity(
            int id, 
            string name, 
            IEnumerable<EntityType> entityTypes, 
            IEnumerable<IEntityComponent> entityComponents,
            IEnumerable<IItem> items,
            IEnumerable<IStat> stats
            ) 
            : base(id, name, items, stats)
        {
            EntityTypes.AddRange(entityTypes);
            EntityComponents.AddRange(entityComponents);
        }

        public bool IsAttackable()
        {
               return EntityComponents.Any(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant)));
        }

        public T FindComponent<T>()
        {
            return (T)EntityComponents.FirstOrDefault(ec => ec.GetType() == typeof(T));

        }
        public TResult FindComponent<TResult, TInterface>()
        {
            return (TResult)EntityComponents.FirstOrDefault(ec => ec.GetType().GetInterfaces().Contains(typeof(TInterface)));

        }

    }
}
