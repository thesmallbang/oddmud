using OddMud.Core.Interfaces;
using OddMud.View.MudLike;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Commands
{
    public static class MudViewCommandBuilder
    {

        public static IViewCommand<IViewItem> BuildMap(IMap map)
        {

            return new MudViewCommands(new List<IViewItem>() { new TextItem() { Text = "Some fake text" } });
        }

    }
}
