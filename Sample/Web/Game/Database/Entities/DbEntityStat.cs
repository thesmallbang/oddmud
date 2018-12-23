using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbEntityStat : BaseEntity
    {

        [ForeignKey("Entity")]
        public int EntityId { get; set; }

        public DbEntity Entity { get; set; }

        public string Name { get; set; }
        public int Base { get; set; }
        public int Current { get; set; }

    }
}
