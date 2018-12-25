using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbClass : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<DbClassAction> Actions {get;set;}


    }
}
