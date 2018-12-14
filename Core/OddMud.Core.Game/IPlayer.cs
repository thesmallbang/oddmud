using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IPlayer
    {
        string Name { get; }
        string NetworkId { get; }

        IMap Map { get; set; }
    }
}
