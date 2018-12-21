using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbPlayerItemStat : BaseEntity
    {

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public DbPlayerItem Item { get; set; }

        public string Name { get; set; }
        public int Base { get; set; }
        public int Current { get; set; }

    }
}
