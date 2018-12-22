using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules;
using OddMud.SampleGame.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{
    public class GridEntity : BasicEntity
    {

        public EntityClasses Class { get; private set; } = EntityClasses.Spitter;
        private bool? _attackableCache;


        public GridEntity(int id, string name, EntityClasses entityClass, IEnumerable<EntityType> entityTypes, IEnumerable<IEntityComponent> entityComponents, IEnumerable<IItem> items) : base(id, name, items)
        {
            Class = entityClass;
            EntityTypes.AddRange(entityTypes);
            EntityComponents.AddRange(entityComponents);
        }

        public bool IsAttackable()
        {
            if (_attackableCache == null)
                _attackableCache =  EntityComponents.Any(ec => ec.GetType().GetInterfaces().Contains(typeof(ICombatant)));

            return _attackableCache.GetValueOrDefault();
        }

    }
}
