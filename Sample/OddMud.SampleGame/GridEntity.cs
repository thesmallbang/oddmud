using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OddMud.SampleGame
{
    public class GridEntity : BasicEntity
    {

        public EntityClasses Class { get; private set; } = EntityClasses.Knight;

        public GridEntity(
            int id, 
            string name, 
            EntityClasses entityClass, 
            IEnumerable<EntityType> entityTypes, 
            IEnumerable<IEntityComponent> entityComponents,
            IEnumerable<IItem> items,
            IEnumerable<IStat> stats
            ) 
            : base(id, name, items, stats)
        {
            Class = entityClass;
            EntityTypes.AddRange(entityTypes);
            EntityComponents.AddRange(entityComponents);
        }

        public bool IsAttackable()
        {
               return EntityComponents.Any(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant)));
        }

    }
}
