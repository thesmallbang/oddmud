using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame
{
    public class GridPlayer : GridEntity, IPlayer
    {
        public GridPlayer(
            int id, 
            string name,
            EntityClasses entityClass, 
            IEnumerable<EntityType> entityTypes, 
            IEnumerable<IEntityComponent> entityComponents,
            IEnumerable<IItem> items, 
            IEnumerable<IStat> stats,
            IMap map)
            : base(id, name, entityClass, entityTypes, entityComponents, items, stats)
        {
            
            Map = map;
        }

        public string TransportId { get; set; }







    }
}
