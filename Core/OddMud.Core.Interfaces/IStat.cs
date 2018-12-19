using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IStat
    {
        string Name { get; }
        int Current { get; }
        int Base { get; }

    }
}
