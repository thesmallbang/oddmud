using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{

    public interface IViewConverter<TOutput>
    {
        TOutput Build(IViewItem viewItem);
    }
}
