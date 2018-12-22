using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IViewCommand<TData>
    {

        IEnumerable<IViewOperation<TData>> Operations { get;  }
     
    }

    public enum ViewOperationType
    {
        Set,
        Append,
    }


    public interface IViewOperation<TData>
    {
        ViewOperationType OperationType { get; }
        IEnumerable<TData> Data { get; }
        string RelatedId { get; }
    }



}
