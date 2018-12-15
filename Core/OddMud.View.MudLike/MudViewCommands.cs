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


}
