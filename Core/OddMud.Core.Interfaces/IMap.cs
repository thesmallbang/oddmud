using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IMap
    {
        int Id { get; }

        string Name { get; }
        string Description { get; }

        IReadOnlyList<IPlayer> Players { get; }

        Task AddPlayerAsync(IPlayer player);
        Task RemovePlayerAsync(IPlayer player);



    }
}
