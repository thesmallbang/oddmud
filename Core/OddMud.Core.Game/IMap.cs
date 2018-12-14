using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IMap
    {
        string Id { get; }

        string Name { get; }
        string Description { get; }

        IReadOnlyList<IPlayer> Players { get; }

        void AddPlayer(IPlayer player);



    }
}
