using OddMud.SampleGame.Misc;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbEntityType : BaseEntity
    {
        public EntityTypes EntityType { get; internal set; }
    }
}