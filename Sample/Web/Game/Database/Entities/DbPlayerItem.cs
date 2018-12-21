using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbPlayerItem : BaseEntity
    {

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public DbPlayer Player { get; set; }


        [ForeignKey("BaseItem")]
        public int BaseItemId { get; set; }

        public DbItem BaseItem { get; set; }
         
        // stats that override the base item stats
        public ICollection<DbPlayerItemStat> Stats { get; set; }


    }
}
