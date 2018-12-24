using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbItem : BaseEntity
    {

        [StringLength(256)] // picked 256 arbitrarily
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("Element")]
        public int? ElementId { get; set; }
        public DbElement Element { get; set; }

        public ICollection<DbItemTypes> ItemTypes { get; set; }
        public ICollection<DbItemStat> Stats { get; set; }



    }
}

