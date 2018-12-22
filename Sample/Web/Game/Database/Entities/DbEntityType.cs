using OddMud.Core.Game;
using OddMud.SampleGame.Misc;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbEntityType : BaseEntity
    {
        public EntityType EntityType { get; internal set; }
    }
}