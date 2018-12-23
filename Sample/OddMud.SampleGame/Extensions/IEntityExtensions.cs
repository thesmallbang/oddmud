using OddMud.Core.Interfaces;
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
        
    }
}
