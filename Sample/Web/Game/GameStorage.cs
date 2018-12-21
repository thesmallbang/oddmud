using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame;
using OddMud.SampleGame.Misc;
using OddMud.Web.Game.Database;
using OddMud.Web.Game.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game
{
    public class GameStorage : IStorage
    {

        private readonly ILogger<GameStorage> _logger;

        public GameStorage(
            ILogger<GameStorage> logger
            )
        {
            _logger = logger;
        }


        // MAPS

        public async Task<IMap> NewMapAsync(IGame game, IMap map)
        {
            var gridMap = (GridMap)map;
            var dbMap = new DbMap()
            {
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Name = map.Name,
                Description = map.Description,
                LocationX = gridMap.Location.X,
                LocationY = gridMap.Location.Y,
                LocationZ = gridMap.Location.Z,
                Exits = gridMap.Exits.Distinct().Select(exit => new DbMapExit() { Direction = (byte)exit }).ToList()
            };

            using (var context = new GameDbContext())
            {
                var savedMap = await context.AddAsync(dbMap);
                await context.SaveChangesAsync();
                return savedMap.Entity.ToMap();
            }
        }

        public async Task UpdateMapsAsync(IGame game, IEnumerable<IMap> maps)
        {

            var toGet = maps.Select(item => item.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbMaps = await context.Maps.Include(m => m.Exits).Where(i => toGet.Contains(i.Id)).ToListAsync();
                if (dbMaps.Count == 0)
                    return;

                foreach (var map in maps)
                {

                    var dbMap = dbMaps.FirstOrDefault(m => m.Id == map.Id);
                    if (dbMap == null)
                        continue;

                    // workaround ..moving on its not important now
                    // i know this is not needed but it was appending to the database instead of clearing and then adding.
                    if (dbMap.Exits.Any())
                    {
                        dbMap.Exits.Clear();
                        dbMap = context.Maps.Update(dbMap).Entity;
                    }

                    var gridMap = (GridMap)map;
                    dbMap.Name = gridMap.Name;
                    dbMap.Description = gridMap.Description;
                    dbMap.LocationX = gridMap.Location.X;
                    dbMap.LocationY = gridMap.Location.Y;
                    dbMap.LocationZ = gridMap.Location.Z;
                    dbMap.ModifiedBy = "nousercontextyet";
                    dbMap.ModifiedDate = DateTime.Now;
                    dbMap.Exits = gridMap.Exits.Distinct().Select(exit => new DbMapExit() { Direction = (byte)exit }).ToList();

                    context.Maps.Update(dbMap);

                }
                await context.SaveChangesAsync();
            }


        }

        public async Task DeleteMapsAsync(IGame game, IEnumerable<IMap> maps)
        {
            var ids = maps.Select(m => m.Id).ToList();

            using (var context = new GameDbContext())
            {

                var dbMaps = await context.Maps.Where(m => ids.Contains(m.Id)).ToListAsync();
                context.Maps.RemoveRange(dbMaps);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<IMap>> LoadMapsAsync(IGame game)
        {

            using (var dbContext = new GameDbContext())
            {
                return await dbContext.Maps.Include(m => m.Exits).Select(m => m.ToMap()).ToListAsync();
            }
        }

        public async Task<IMap> LoadMapAsync(IGame game, int id)
        {
            using (var dbContext = new GameDbContext())
            {
                var dbMap = await dbContext.Maps.Include(m => m.Exits).FirstOrDefaultAsync(m => m.Id == id);
                return dbMap.ToMap();
            }
        }


        // PLAYERS
        public async Task<IPlayer> LoadPlayerAsync(IGame game, string name, string pass)
        {
            using (var dbContext = new GameDbContext())
            {
                var dbPlayer = await dbContext.Players
                    .Include(player => player.Items).ThenInclude(item => item.Stats)
                    .Include(player => player.Items).ThenInclude(item => item.BaseItem).ThenInclude(baseitem => baseitem.ItemTypes)
                    .FirstOrDefaultAsync(o => o.Name.ToLower().Trim() == name.ToLower().Trim());
                if (dbPlayer == null)
                {
                    _logger.LogWarning($"Attempted to load player that doesn't exist");
                    return null;
                }
                var isAuthenticated = PasswordStorage.VerifyPassword(pass, dbPlayer.Password);
                if (!isAuthenticated)
                {
                    return null;
                }

                // need to refactor out these conversions 
                return dbPlayer.ToPlayer(game);
            };
        }


        public async Task<IPlayer> NewPlayerAsync(IGame game, IPlayer player, string pass)
        {
            try
            {
                var gridPlayer = (GridPlayer)player;
                var dbPlayer = new DbPlayer()
                {
                    RecordBy = "notlinktoausercontextyet",
                    RecordDate = DateTimeOffset.Now,
                    Name = player.Name,
                    Password = PasswordStorage.CreateHash(pass),
                    Class = gridPlayer.Class,
                    Items = gridPlayer.Items.Select(i => new DbPlayerItem()
                    {
                        PlayerId = player.Id,
                        BaseItemId = i.Id,
                        Stats = i.Stats.Select(s => new DbPlayerItemStat() { Base = s.Base, Current = s.Current, Name = s.Name }).ToList()

                    }).ToList()
                };
                using (var context = new GameDbContext())
                {
                    var trackedPlayer = context.Players.Add(dbPlayer);
                    await context.SaveChangesAsync();
                    return trackedPlayer.Entity.ToPlayer(game);
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning("new player rejected: " + ex.Message);
                return null;
            }

        }

        public async Task UpdatePlayersAsync(IGame game, IEnumerable<IPlayer> players)
        {
            var toGet = players.Select(item => item.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbPlayers = await context.Players.Where(s => toGet.Contains(s.Id)).ToListAsync();
                if (dbPlayers.Count == 0)
                    return;

                foreach (GridPlayer player in players)
                {

                    var dbPlayer = dbPlayers.FirstOrDefault(s => s.Id == player.Id);
                    if (dbPlayer == null)

                        dbPlayer.Name = player.Name;
                    dbPlayer.LastMap = player.Map.Id;


                    context.Players.Update(dbPlayer);

                }
                await context.SaveChangesAsync();
            }
        }

        public async Task DeletePlayersAsync(IGame game, IEnumerable<IPlayer> players)
        {
            var ids = players.Select(i => i.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbPlayers = await context.Players.Where(i => ids.Contains(i.Id)).ToListAsync();
                if (dbPlayers.Count == 0)
                    return;

                context.Players.RemoveRange(dbPlayers);
                await context.SaveChangesAsync();
            }
        }


        // ITEMS
        public async Task<IItem> NewItemAsync(IGame game, IItem item)
        {
            // use some sort of mapper? to clean this up?
            var gridItem = (GridItem)item;
            var dbItem = new DbItem()
            {
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Name = item.Name,
                Description = item.Description,
                ItemTypes = gridItem.ItemTypes.Select(it => new DbItemTypes() { ItemType = it }).ToList()
            };

            using (var context = new GameDbContext())
            {
                var trackedItem = await context.Items.AddAsync(dbItem);
                await context.SaveChangesAsync();
                return trackedItem.Entity.ToItem();
            }

        }

        public async Task UpdateItemsAsync(IGame game, IEnumerable<IItem> items)
        {

            using (var context = new GameDbContext())
            {

                var itemsToGet = items.Select(item => item.Id).ToList();

                var dbItems = await context.Items.Include(i => i.ItemTypes).Where(i => itemsToGet.Contains(i.Id)).ToListAsync();
                if (dbItems.Count == 0)
                    return;

                foreach (var item in items)
                {
                    var gridItem = (GridItem)item;
                    var dbItem = dbItems.FirstOrDefault(i => i.Id == item.Id);
                    if (gridItem == null)
                        continue;

                    dbItem.Name = gridItem.Name;
                    dbItem.Description = gridItem.Description;

                    if (dbItem.ItemTypes.Any())
                    {
                        dbItem.ModifiedBy = "nousercontextyet";
                        dbItem.ModifiedDate = DateTime.Now;
                        dbItem.ItemTypes.Clear();
                        dbItem = context.Items.Update(dbItem).Entity;
                    }

                    dbItem.ItemTypes = gridItem.ItemTypes.Select(it => new DbItemTypes() { ItemType = it }).ToList();
                    context.Items.Update(dbItem);
                }
                await context.SaveChangesAsync();
            }

        }

        public async Task DeleteItemsAsync(IGame game, IEnumerable<IItem> items)
        {
            var ids = items.Select(i => i.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbItems = await context.Items.Where(i => ids.Contains(i.Id)).ToListAsync();
                if (dbItems.Count == 0)
                    return;

                context.Items.RemoveRange(dbItems);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<IItem>> LoadItemsAsync(IGame game)
        {
            using (var dbContext = new GameDbContext())
            {
                var items = await dbContext.Items.Include(i => i.ItemTypes).Include(i => i.Stats)
                    .Select(db => db.ToItem(null)).ToListAsync();
                _logger.LogInformation("Loaded items");
                return items;
            }
        }


        // SPAWNERS
        public async Task<IEnumerable<ISpawner>> LoadSpawnersAsync(IGame game)
        {
            using (var dbContext = new GameDbContext())
            {
                var dbSpawners = await dbContext.Spawners.Where(s => s.Enabled).ToListAsync();
                return dbSpawners.Select(s => s.ToSpawner()).ToList();
            }
        }

        public async Task<ISpawner> NewSpawnerAsync(IGame game, ISpawner spawner)
        {
            _logger.LogInformation("Saving new spawner");
            var sSpawner = (SingletonSpawner)spawner;

            // use some sort of mapper? to clean this up?
            var dbSpawner = new DbSpawner()
            {
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Enabled = true,
                EntityId = sSpawner.EntityId
            };

            using (var context = new GameDbContext())
            {
                var tracked = await context.Spawners.AddAsync(dbSpawner);
                await context.SaveChangesAsync();
                return tracked.Entity.ToSpawner();
            }
        }

        public async Task UpdateSpawnersAsync(IGame game, IEnumerable<ISpawner> spawners)
        {
            var toGet = spawners.Select(item => item.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbSpawners = await context.Spawners.Where(s => toGet.Contains(s.Id)).ToListAsync();
                if (dbSpawners.Count == 0)
                    return;

                foreach (GridSpawner spawner in spawners)
                {

                    var dbSpawner = dbSpawners.FirstOrDefault(s => s.Id == spawner.Id);
                    if (dbSpawner == null)
                        continue;
                    // should we save as new here? .. right now i wont so the current world can hold temporary spawners from events etc etc. 

                    dbSpawner.MapId = spawner.MapId;
                    dbSpawner.EntityId = spawner.Id;
                    dbSpawner.ModifiedBy = "nousercontextyet";
                    dbSpawner.ModifiedDate = DateTime.Now;
                    dbSpawner.SpawnType = (byte)spawner.SpawnType;

                    context.Spawners.Update(dbSpawner);

                }
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSpawnersAsync(IGame game, IEnumerable<ISpawner> spawners)
        {
            var ids = spawners.Select(i => i.Id).ToList();

            using (var context = new GameDbContext())
            {
                var dbSpawners = await context.Spawners.Where(i => ids.Contains(i.Id)).ToListAsync();
                if (dbSpawners.Count == 0)
                    return;

                context.Spawners.RemoveRange(dbSpawners);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEntity> NewEntityAsync(IGame game, IEntity entity)
        {

            try
            {
                var gridEntity = (GridEntity)entity;
                var dbEntity = new DbEntity()
                {
                    RecordBy = "notlinktoausercontextyet",
                    RecordDate = DateTimeOffset.Now,
                    Name = gridEntity.Name,
                    Class = gridEntity.Class,
                    Items = gridEntity.Items.Select(i => new DbEntityItem()
                    {
                        BaseItemId = i.Id
                    }).ToList()
                };
                using (var context = new GameDbContext())
                {
                    var tracked = context.Entities.Add(dbEntity);
                    await context.SaveChangesAsync();
                    return tracked.Entity.ToEntity(game);
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning("new entity rejected: " + ex.Message);
                return null;
            }

        }

        public Task UpdateEntitiesAsync(IGame game, IEnumerable<IEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntitiesAsync(IGame game, IEnumerable<IEntity> entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> LoadEntitiesAsync(IGame game)
        {
            throw new NotImplementedException();
        }
    }
}
