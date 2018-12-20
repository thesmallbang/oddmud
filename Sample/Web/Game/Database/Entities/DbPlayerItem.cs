using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbPlayerItems : BaseEntity
    {

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public DbItem Item { get; set; }
 
        


    }
}
