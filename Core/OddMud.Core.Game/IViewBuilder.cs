using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{

    public interface IViewBuilder<TOutput>
    {
        TOutput Build(IViewItem viewItem);
    }
}
