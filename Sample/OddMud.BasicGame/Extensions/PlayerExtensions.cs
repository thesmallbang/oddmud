using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.BasicGame.Extensions
{
    public static class PlayerExtensions
    {
        public static IPlayer GetPlayerByTransportId(this IReadOnlyList<IPlayer> players, string transportId)
        {
            return players.FirstOrDefault(player => player.TransportId == transportId);
        }

        public static IPlayer GetPlayerByName(this IReadOnlyList<IPlayer> players, string name)
        {
            return players.FirstOrDefault(player => player.Name == name);
        }
    }
}
