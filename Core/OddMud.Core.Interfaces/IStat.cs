using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IStat
    {
        string Name { get; }
        int Current { get; }
        int Base { get; }

        event Func<IStat, int, Task> StatChanged;

        Task ApplyToCurrentAsync(int modifier);

    }
}
