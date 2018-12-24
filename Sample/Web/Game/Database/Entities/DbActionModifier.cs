using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{

    public class DbActionModifier : BaseEntity
    {
        
        [ForeignKey("Action")]
        public int ActionId { get; set; }

        public DbAction Action { get; set; }

        public string Name { get; set; }

        // flat, % etc
        public ActionModifierType ModifierType { get; set; }

        // other, caster
        public ModifierTargetTypes TargetType { get; set; }

        public int Min { get; set; }
        public int Max { get; set; }


    }
 }
