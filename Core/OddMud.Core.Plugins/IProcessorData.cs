using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Plugins
{
   public interface IProcessorData<TData>
    {
        TData Data { get; }
        string TransportId { get; }
        bool Handled { get; set; }
              
    }
}
