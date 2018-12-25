using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbClassAction : BaseEntity
    {

        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public DbClass Class { get; set; }


        [ForeignKey("Action")]
        public int ActionId { get; set; }
        public DbAction Action { get; set; }

        public ICollection<DbClassActionRequirement> Requirements { get; set; }

        public ICollection<DbClassActionConsumeModifier> Consumes { get; set; }



    }
}
