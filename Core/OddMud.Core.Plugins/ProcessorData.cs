using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Plugins
{
    public class ProcessorData<TData> : IProcessorData<TData>
    {
        public TData Data { get; }
        public string TransportId { get; }
        public bool Handled { get; set; }

        public ProcessorData(TData data, string networkId)
        {
            Data = data;
            TransportId = networkId;
        }
    }
}
