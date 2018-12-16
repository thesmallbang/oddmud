using OddMud.Core.Interfaces;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.BasicGame.Extensions
{
    public static class MudLikeCommandBuilderExtensions
    {
        
        public static MudLikeCommandBuilder GetPlayersUpdate(this MudLikeCommandBuilder builder, IEnumerable<IPlayer> players)
        {
            return builder.AddText("players: ")
           .AddText(string.Join(",", players.Select(p => p.Name)), TextColor.Gray);

        }
    }
}
