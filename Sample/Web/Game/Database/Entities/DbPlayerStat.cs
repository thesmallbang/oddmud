using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbPlayerStat : BaseEntity
    {

        [ForeignKey("Player")]
        public int PlayerId { get; set; }

        public DbPlayer Player { get; set; }

        public string Name { get; set; }
        public int Base { get; set; }
        public int Current { get; set; }

    }
}
