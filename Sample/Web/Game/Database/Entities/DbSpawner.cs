using OddMud.Core.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbSpawner : BaseEntity
    {
        // controls wether we will pass it to the game with the load spawners call
        public bool Enabled { get; set; }

        public int SpawnType { get; set; }

        public int EntityId { get; set; }
        
        [ForeignKey("Map")]
        public int MapId { get; set; }

        public DbMap Map { get; set; }


    }
}

