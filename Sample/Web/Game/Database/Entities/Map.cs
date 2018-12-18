using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class Map : BaseEntity
    {
      
        [StringLength(256)] // picked 256 arbitrarily
        public string Name { get; set; }

        public string Description { get; set; }

        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int LocationZ { get; set; }

        public ICollection<MapExit> Exits { get; set; }

    }
}

