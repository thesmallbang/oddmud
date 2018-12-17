using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeCommandBuilder
    {


        public static MudLikeCommandBuilder Start()
        {
            return new MudLikeCommandBuilder();
        }

        private List<IViewItem> _commands = new List<IViewItem>();


        public IViewCommand<IViewItem> Build(ViewCommandType commandType = ViewCommandType.Set)
        {
            switch (commandType)
            {
                case ViewCommandType.Set:
                    return new MudViewCommands(_commands);
                case ViewCommandType.Append:
                    return new MudAppendViewCommands(_commands);
                case ViewCommandType.Replace:
                    return new MudReplaceViewCommands(_commands);
            }
            return null;
        }

        public MudLikeCommandBuilder AddText(string message, TextColor color = TextColor.Normal, TextSize size = TextSize.Normal)
        {
            _commands.Add(new TextItem(message, color, size));
            return this;
        }

        public MudLikeCommandBuilder AddTextLine(string message, TextColor color = TextColor.Normal, TextSize size = TextSize.Normal)
        {
            AddText(message, color, size);
            AddLineBreak();
            return this;
        }

        public MudLikeCommandBuilder AddLineBreak()
        {
            _commands.Add(new LineBreakItem());
            return this;
        }

    }
}
