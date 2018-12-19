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


        Task<IEnumerable<IItem>> LoadItemsAsync();
        Task NewItemAsync(IItem item);
        Task UpdateItemAsync(IItem item);
        Task DelteItemAsync(IItem item);

        Task<IEnumerable<ISpawner>> LoadSpawnersAsync();
        Task NewSpawnerAsync(ISpawner spawner);
        Task UpdateSpawnerAsync(ISpawner spawner);
        Task DelteSpawnerAsync(ISpawner spawner);


    }
}
