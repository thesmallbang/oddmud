using OddMud.SampleGame;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbPlayer : BaseEntity
    {
        
        public string Name { get; set; }
        public string Password { get; set; }
        public int LastMap { get; set; }

        [ForeignKey("Class")]
        public int? ClassId { get; set; }

        public DbClass Class { get; set; }


        public ICollection<DbPlayerItem> Items { get; set; }
        public ICollection<DbPlayerStat> Stats { get; set; }


    }
}

