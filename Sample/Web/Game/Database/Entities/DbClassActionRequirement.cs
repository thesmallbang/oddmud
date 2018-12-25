using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbClassActionRequirement : BaseEntity
    {

        [ForeignKey("ClassAction")]
        public int ClassActionId { get; set; }

        public DbClassAction ClassAction { get; set; }

        // level, str, etc..
        public string Name { get; set; }

        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}
