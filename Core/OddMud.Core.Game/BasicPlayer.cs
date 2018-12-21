
using OddMud.Core.Interfaces;

namespace OddMud.Core.Game
{
    public class BasicPlayer : BasicEntity, IPlayer
    {
        public BasicPlayer(int id, string name, System.Collections.Generic.IEnumerable<IItem> items) : base(id, name, items)
        {
            
        }



        public string TransportId { get; set; }
                           

       

    }
}
