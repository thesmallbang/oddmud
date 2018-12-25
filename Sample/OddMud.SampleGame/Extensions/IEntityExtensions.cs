using OddMud.Core.Interfaces;
using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.Extensions
{
    public static class IEntityExtensions
    {
        public static bool IsPlayer(this IEntity entity)
        {
            return entity.GetType().GetInterfaces().Contains(typeof(IPlayer));
        }

        public static bool CurrentStatsAtLeast(this IEntity entity, IEnumerable<IActionModifier> modifiers)
        {

            foreach (var modifier in modifiers)
            {
                var modifierStat = entity.Stats.FirstOrDefault(s => s.Name == modifier.Name);
                if (Math.Abs(modifier.Value) > modifierStat.Value)
                    return false;

            }

            return true;
        }

        public static bool BaseStatsAtLeast(this IEntity entity, IEnumerable<IActionModifier> modifiers)
        {

            foreach (var modifier in modifiers)
            {
                var modifierStat = entity.Stats.FirstOrDefault(s => s.Name == modifier.Name);
                if (Math.Abs(modifier.Value) > modifierStat.Base)
                    return false;

            }

            return true;
        }

        public static bool CurrentStatAtLeast(this IEntity entity, IActionModifier modifier)
        {
            return CurrentStatsAtLeast(entity, new List<IActionModifier>() { modifier });
        }
        public static bool BaseStatAtLeast(this IEntity entity, IActionModifier modifier)
        {
            return BaseStatsAtLeast(entity, new List<IActionModifier>() { modifier });
        }


    }
}
