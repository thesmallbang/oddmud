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

        public static MudLikeCommandBuilder AddPlayers(this MudLikeCommandBuilder builder, IEnumerable<IPlayer> players)
        {
            return builder.StartContainer("playerlist").AddText("players: ")
           .AddText(string.Join(",", players.Select(p => p.Name)), TextColor.Gray)
           .EndContainer();
        }
        public static MudLikeCommandBuilder AddWorldDate(this MudLikeCommandBuilder builder, DateTime dateTime)
        {
            return builder.StartContainer("dateview").AddText("date: ")
                            .AddTextLine(dateTime.ToString("D"), TextColor.Fuschia, TextSize.Small)
                            .EndContainer();

        }

        public static MudLikeCommandBuilder AddMap(this MudLikeCommandBuilder builder, GridMap map)
        {
            return builder
                .StartContainer("mapdata").AddTextLine(map.ToString(), TextColor.Teal, TextSize.Strong)
                .AddTextLine(map.Description, size: TextSize.Large)
                .EndContainer()
                .StartContainer("mapexitdata")
                .AddText("exits: ")
                .AddTextLine(string.Join(",", map.Exits.Select(o => o.ToString().ToLower())), TextColor.Green)
                .EndContainer();
        }

    }
}
