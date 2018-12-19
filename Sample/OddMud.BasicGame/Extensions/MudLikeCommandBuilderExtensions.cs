using OddMud.Core.Interfaces;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.Extensions
{
    public static class MudLikeCommandBuilderExtensions
    {

        public static MudLikeCommandBuilder AddPlayers(this MudLikeCommandBuilder builder, IEnumerable<IPlayer> players)
        {
            return builder.StartContainer("playerlist").AddText("players: ")
           .AddText(string.Join(",", players.Select(p => p.Name)), TextColor.Gray)
           .EndContainer("playerlist");
        }
        public static MudLikeCommandBuilder AddWorldDate(this MudLikeCommandBuilder builder, DateTime dateTime)
        {
            return builder
                .StartContainer("dateview")
                .AddTextLine(dateTime.ToString("D"), TextColor.Fuschia, TextSize.Small)
                .EndContainer("dateview");

        }

        public static MudLikeCommandBuilder AddMap(this MudLikeCommandBuilder builder, GridMap map, bool includePlayers = false)
        {
            builder
                .StartContainer("mapdata")
                .AddText($"{map.Id} ", TextColor.Gray, TextSize.Small)
                .AddTextLine(map.Name, color: TextColor.Aqua, size: TextSize.Strong)
                .AddTextLine(map.Description, size: TextSize.Strong)
                .AddText("Exits ")
                .AddTextLine(string.Join(",", map.Exits.Select(o => o.ToString().ToLower())), TextColor.Green)
                .EndContainer("mapdata");
            if (includePlayers)
                builder.AddPlayers(map.Players);

            return builder;
        }

    }
}
