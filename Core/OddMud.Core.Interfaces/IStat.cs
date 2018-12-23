using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IStat : IProperty<int>
    {
    
        int Base { get; }

        event Func<IStat, int, Task> StatChanged;

        Task ApplyAsync(int modifier);

        Task Fill();

    }


}
