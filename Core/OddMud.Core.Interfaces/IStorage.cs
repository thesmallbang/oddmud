using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IStorage
    {

        Task<IEnumerable<IPlayer>> LoadPlayersAsync();
        Task<IEnumerable<IMap>> LoadMapsAsync();

        Task SaveMapAsync(IMap map);
        Task SavePlayerAsync(IPlayer player);
        Task SavePlayersAsync(IEnumerable<IPlayer> players);


    }
}
