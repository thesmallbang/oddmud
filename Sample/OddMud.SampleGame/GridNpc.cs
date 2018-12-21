using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGame
{
    public class GridNpc : BasicEntity
    {
        public GridNpc(int id, string name, IEnumerable<IItem> items) : base(id, name, items)
        {
        }

        public EntityClasses Class { get; private set; } = EntityClasses.Spitter;
        
        
    }
}
