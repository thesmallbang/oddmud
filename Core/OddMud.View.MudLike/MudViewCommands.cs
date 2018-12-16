using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudViewCommands : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Set;

        public IEnumerable<IViewItem> Data { get; private set; }

        public MudViewCommands(IEnumerable<IViewItem> viewItems)
        {
            Data = viewItems;
        }
    }

    public class MudViewCommand : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Set;

        public IEnumerable<IViewItem> Data { get; private set; }

        public void SetData(IEnumerable<IViewItem> data)
        {
            Data = data;
        }
    }

    public class MudAppendViewCommands : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Append;

        public IEnumerable<IViewItem> Data { get; private set; }

        public MudAppendViewCommands(IEnumerable<IViewItem> viewItems)
        {
            Data = viewItems;
        }
    }


}
