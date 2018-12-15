using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public static class MudViewCommandBuilder
    {

        public static IViewCommand<IViewItem> BuildMap(IMap map)
        {

            return new MudViewCommands(new List<IViewItem>() { new TextItem() { Text = "Some fake text" } });
        }

    }
}
