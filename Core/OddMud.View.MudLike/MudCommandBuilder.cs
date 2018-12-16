using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeCommandBuilder
    {

        private List<IViewItem> _commands = new List<IViewItem>();


        public IViewCommand<IViewItem> Build(bool append = false)
        {
            if (!append)
                return new MudViewCommands(_commands);
            else
                return new MudAppendViewCommands(_commands);
        }

        public MudLikeCommandBuilder AddText(string message, TextColor color = TextColor.Normal, TextSize size = TextSize.Normal)
        {
            _commands.Add(new TextItem(message, color, size));
            return this;
        }
        public MudLikeCommandBuilder AddLineBreak()
        {
            _commands.Add(new LineBreakItem());
            return this;
        }

    }
}
