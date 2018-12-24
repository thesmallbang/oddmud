using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbElementRange : BaseEntity
    {

        [ForeignKey("Element")]
        public int ElementId { get; set; }

        public DbElement Element { get; set; }

        public int? Min { get; set; }
        public int? Max { get; set; }
        public string Text { get; set; }
        public TextColor TextColor { get; set; }

    }
}
