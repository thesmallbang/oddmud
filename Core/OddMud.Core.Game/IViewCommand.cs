using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IViewCommand<TData>
    {
        ViewCommandType CommandType { get; }
        IEnumerable<TData> Data { get; }
    }

    public enum ViewCommandType
    {
        Set,
        Append
    }
}
