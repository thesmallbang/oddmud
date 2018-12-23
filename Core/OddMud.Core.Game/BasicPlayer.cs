
using OddMud.Core.Interfaces;
using System.Collections.Generic;

namespace OddMud.Core.Game
{
    public class BasicPlayer : BasicEntity, IPlayer
    {
        public BasicPlayer(int id, string name, IEnumerable<IItem> items, IEnumerable<IStat> stats) : base(id, name, items, stats)
        {
            
        }



        public string TransportId { get; set; }
                           

       

    }
}
