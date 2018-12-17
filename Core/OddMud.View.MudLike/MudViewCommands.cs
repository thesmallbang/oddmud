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

        public string ReplaceId => "";

        public MudViewCommands(IEnumerable<IViewItem> viewItems)
        {
            Data = viewItems;
        }
    }

    public class MudViewCommand : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Set;

        public IEnumerable<IViewItem> Data { get; private set; }

        public string ReplaceId => "";

        public void SetData(IEnumerable<IViewItem> data)
        {
            Data = data;
        }
    }

    public class MudAppendViewCommands : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Append;

        public IEnumerable<IViewItem> Data { get; private set; }

        public string ReplaceId => "";

        public MudAppendViewCommands(IEnumerable<IViewItem> viewItems)
        {
            Data = viewItems;
        }
    }
    public class MudReplaceViewCommands : IViewCommand<IViewItem>
    {
        public ViewCommandType CommandType => ViewCommandType.Replace;
        public string ReplaceId { get; set; }

        public IEnumerable<IViewItem> Data { get; private set; }

        public MudReplaceViewCommands(string replaceId, IEnumerable<IViewItem> viewItems)
        {
            ReplaceId = replaceId;
            Data = viewItems;
        }
    }


}
