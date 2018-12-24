using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database.Entities
{
    public class DbAction : BaseEntity
    {

        public string Name { get; set; }

        // single target, duration spell, etc etc
        public byte ActionType { get; set; }

        // physical, fire, etc
        public DbElement ElementType { get; set; }

        // self, selfae, friend, friendae, enemy,  enemyae
        public TargetTypes TargetType { get; set; }

        // fireball .. single -> fire -> enemy [mods]-hp
        // heal .. single -> nature -> self [mods]+hp
        // group heal .. single -> nature -> friendae  [mods]+hp each
        // chaos room .. single -> chaos -> enemyae [mods]-mp each

        // modifiers

        public ICollection<DbActionModifier> Modifiers { get; set; }


    }
 }
