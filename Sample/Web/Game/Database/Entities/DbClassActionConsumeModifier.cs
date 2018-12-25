using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbClassActionConsumeModifier : BaseEntity
    {

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public DbClass Class { get; set; }

        // mana, stamina etc
        public string Name { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
