using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{

    public interface IGameModule
    {
        Task TickAsync();
    }

    public interface IGameModule<T> : IGameModule
    {

    }
}
