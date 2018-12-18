using System;
using System.ComponentModel.DataAnnotations;

namespace OddMud.Web.Game.Database.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        [StringLength(100)]
        public string RecordBy { get; set; }
        public DateTimeOffset? RecordDate { get; set; }
    }
}

