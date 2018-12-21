using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Interfaces
{
    public interface IStorage
    {


        Task<IMap> NewMapAsync(IGame game,IMap map);
        Task UpdateMapsAsync(IGame game, IEnumerable<IMap> maps);
        Task DeleteMapsAsync(IGame game, IEnumerable<IMap> maps);
        Task<IEnumerable<IMap>> LoadMapsAsync(IGame game);


        Task<IPlayer> NewPlayerAsync(IGame game, IPlayer player, string pass);
        Task UpdatePlayersAsync(IGame game, IEnumerable<IPlayer> players);
        Task DeletePlayersAsync(IGame game, IEnumerable<IPlayer> players);
        Task<IPlayer> LoadPlayerAsync(IGame game, string name, string pass);



        Task<IItem> NewItemAsync(IGame game, IItem item);
        Task UpdateItemsAsync(IGame game, IEnumerable<IItem> items);
        Task DeleteItemsAsync(IGame game, IEnumerable<IItem> items);
        Task<IEnumerable<IItem>> LoadItemsAsync(IGame game);



        Task<IEntity> NewEntityAsync(IGame game, IEntity entity);
        Task UpdateEntitiesAsync(IGame game, IEnumerable<IEntity> entity);
        Task DeleteEntitiesAsync(IGame game, IEnumerable<IEntity> entity);
        Task<IEnumerable<IEntity>> LoadEntitiesAsync(IGame game);



        Task<IEnumerable<ISpawner>> LoadSpawnersAsync(IGame game);
        Task<ISpawner> NewSpawnerAsync(IGame game, ISpawner spawner);
        Task UpdateSpawnersAsync(IGame game, IEnumerable<ISpawner> spawners);
        Task DeleteSpawnersAsync(IGame game, IEnumerable<ISpawner> spawners);


    }
}
