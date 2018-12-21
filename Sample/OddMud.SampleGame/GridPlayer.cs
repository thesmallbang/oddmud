using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame
{
    public class GridPlayer : BasicPlayer
    {
        public EntityClasses Class { get; private set; } = EntityClasses.Spitter;


        public GridPlayer(int id, string name, IEnumerable<IItem> items) : base(id, name, items)
        {
        }

        public GridPlayer(int id, string name, EntityClasses playerClass, IMap map, IEnumerable<IItem> items) : base(id, name, items)
        {
            Class = playerClass;
            Map = map;
        }




    }
}
