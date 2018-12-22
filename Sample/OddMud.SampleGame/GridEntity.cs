using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules;
using OddMud.SampleGame.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{
    public class GridEntity : BasicEntity
    {
        public GridEntity(int id, string name, EntityClasses entityClass,IEnumerable<EntityTypes> entityTypes, IEnumerable<IItem> items) : base(id, name, items)
        {
            Class = entityClass;
            EntityTypes.AddRange(entityTypes);
        }

        public EntityClasses Class { get; private set; } = EntityClasses.Spitter;

        public List<EntityTypes> EntityTypes = new List<EntityTypes>();
        public List<IEntityComponent> EntityComponents = new List<IEntityComponent>();
                
    }
}
