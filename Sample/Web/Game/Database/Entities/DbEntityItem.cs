using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbEntityItem : BaseEntity
    {

        [ForeignKey("Entity")]
        public int EntityId { get; set; }
        public DbEntity Entity { get; set; }


        [ForeignKey("BaseItem")]
        public int BaseItemId { get; set; }

        public DbItem BaseItem { get; set; }


    }
}
