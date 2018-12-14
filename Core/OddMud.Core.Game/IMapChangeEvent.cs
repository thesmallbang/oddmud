using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IMapChangeEvent
    {
        IPlayer Player { get; }
        IMap OldMap { get; }
        IMap NewMap { get; }
    }
}
