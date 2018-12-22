using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeViewCommand : IViewCommand<IViewItem>
    {
        public IEnumerable<IViewOperation<IViewItem>> Operations { get; set; }

        public MudLikeViewCommand(IEnumerable<IViewOperation<IViewItem>> operations)
        {
            Operations = operations;
        }
    }
}
