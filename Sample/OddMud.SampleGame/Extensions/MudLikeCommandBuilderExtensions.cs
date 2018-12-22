using OddMud.Core.Interfaces;
using OddMud.SampleGame.Misc;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame.Extensions
{
    public static class MudLikeCommandBuilderExtensions
    {

        public static MudLikeOperationBuilder AddPlayers(this MudLikeOperationBuilder builder, IEnumerable<IPlayer> players)
        {
            return builder.StartContainer("playerlist").AddText("players: ")
           .AddText(string.Join(",", players.Select(p => p.Name)), TextColor.Gray)
           .EndContainer("playerlist");
        }

        public static MudLikeOperationBuilder AddItems(this MudLikeOperationBuilder builder, IEnumerable<IItem> items)
        {
            return builder.StartContainer("itemlist").AddText("ground items: ")
           .AddText(string.Join(",", items.Select(p => p.Name)), TextColor.Olive)
           .EndContainer("itemlist");
        }


        public static MudLikeOperationBuilder AddEntities(this MudLikeOperationBuilder builder, IEnumerable<IEntity> entities)
        {
            return builder.StartContainer(MudContainers.EntityList.ToString()).AddText("entities: ")
           .AddText(string.Join(",", entities.Select(p => p.Name)), TextColor.Red)
           .EndContainer(MudContainers.EntityList.ToString());
        }


        public static MudLikeOperationBuilder AddWorldDate(this MudLikeOperationBuilder builder, DateTime dateTime)
        {
            return builder
                .StartContainer("dateview")
                .AddTextLine(dateTime.ToString("D"), TextColor.Fuschia, TextSize.Small)
                .EndContainer("dateview");

        }

        public static MudLikeOperationBuilder AddMap(this MudLikeOperationBuilder builder, GridMap map, bool includePlayers = false)
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
