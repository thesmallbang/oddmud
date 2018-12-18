using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IStorage
    {

        Task<IPlayer> LoadPlayerAsync(string name, string pass);
        Task<IEnumerable<IMap>> LoadMapsAsync();


        Task<IMap> LoadMapAsync(int id);
        Task<int> NewMapAsync(IMap map);
        Task DeleteMapAsync(IMap map);
        Task UpdateMapAsync(IMap map);

        Task<bool> NewPlayerAsync(IPlayer player, string pass);
        Task UpdatePlayerAsync(IPlayer player);
        Task UpdatePlayersAsync(IEnumerable<IPlayer> players);


    }
}
