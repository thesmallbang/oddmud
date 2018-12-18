using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbMapExit : BaseEntity
    {

        [ForeignKey("Map")]
        public int MapId { get; set; }

        public DbMap Map { get; set; }

        public byte Direction { get; set; }

    }
}

