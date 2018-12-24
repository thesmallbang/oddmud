using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class FactionProperty : BasicProperty<List<IEntity>>
    {
        public FactionProperty(string name) : base(name)
        {
        }

        public FactionProperty(string name, List<IEntity> value) : base(name, value)
        {
        }
    }
}
