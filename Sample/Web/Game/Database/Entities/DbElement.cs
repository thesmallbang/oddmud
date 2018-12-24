using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbElement : BaseEntity
    {
        public string Name { get; set; }

        public TextColor TextColor { get; set; }

        public ICollection<DbElementRange> Ranges { get; set; }


    }
}
