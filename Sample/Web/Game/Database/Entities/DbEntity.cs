using OddMud.SampleGame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{


    public class DbEntity : BaseEntity // baseentity is related to entity framework (sql) not the game entity
    {


        public string Name { get; set; }

        [ForeignKey("Class")]
        public int? ClassId { get; set; }
        public DbClass Class { get; set; }
        
        // when you have a custom class to provide non standard intelligence
        // see dbconverter for additinal changes needed
        public int? IntelligenceOverride { get; set; }

        [ForeignKey("LootTable")]
        public int? LootTableId { get; set; }

        public DbLootTable LootTable { get; set; }

        public ICollection<DbEntityItem> Items { get; set; }
        public ICollection<DbEntityStat> Stats { get; set; }

        public ICollection<DbEntityType> EntityTypes { get; set; }



    }
}
