using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.BasicGame.Extensions
{
    public static class PlayerExtensions
    {
        public static IPlayer GetPlayerByTransportId(this IEnumerable<IPlayer> players, string transportId)
        {
            return players.FirstOrDefault(player => player.TransportId == transportId);
        }

        public static IPlayer GetPlayerByName(this IEnumerable<IPlayer> players, string name)
        {
            return players.FirstOrDefault(player => player.Name == name);
        }

        public static IEnumerable<IPlayer> Except(this IEnumerable<IPlayer> players, IPlayer player)
        {
            return players.Where(p => p != player).ToList();
        }


    }
}
