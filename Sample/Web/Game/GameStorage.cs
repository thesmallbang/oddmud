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


        public async Task UpdateMapAsync(IMap map)
        {
            using (var context = new GameDbContext())
            {
                var dbMap = await context.Maps.Include(p => p.Exits).FirstOrDefaultAsync(o => o.Id == map.Id);

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
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> NewMapAsync(IMap map)
        {
            var gridMap = (GridMap)map;
            var dbMap = new DbMap()
            {
                Name = map.Name,
                Description = map.Description,
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                LocationX = gridMap.Location.X,
                LocationY = gridMap.Location.Y,
                LocationZ = gridMap.Location.Z,
                Exits = gridMap.Exits.Distinct().Select(exit => new DbMapExit() { Direction = (byte)exit }).ToList()
            };

            using (var context = new GameDbContext())
            {
                var savedMap = await context.AddAsync(dbMap);
                await context.SaveChangesAsync();
                return savedMap.Entity.Id;
            }
        }

        public async Task DeleteMapAsync(IMap map)
        {

            using (var context = new GameDbContext())
            {

                var dbMap = await context.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
                context.Maps.Remove(dbMap);
                await context.SaveChangesAsync();

            }
        }

        public async Task<IEnumerable<IMap>> LoadMapsAsync()
        {

            using (var dbContext = new GameDbContext())
            {
                return await dbContext.Maps.Select(db =>
                new GridMap(
                    db.Id, db.Name, db.Description,
                    new GridLocation(db.LocationX, db.LocationY, db.LocationZ),
                    db.Exits.Select(e => (Exits)e.Direction).ToList()
                    )).ToListAsync();
            }
        }
        public async Task<IMap> LoadMapAsync(int id)
        {
            using (var dbContext = new GameDbContext())
            {
                return await dbContext.Maps.Where(m => m.Id == id).Select(db =>
                  new GridMap(
                      db.Id, db.Name, db.Description,
                      new GridLocation(db.LocationX, db.LocationY, db.LocationZ),
                      db.Exits.Select(e => (Exits)e.Direction).ToList()
                      )).FirstOrDefaultAsync();
            }
        }

        public async Task<IPlayer> LoadPlayerAsync(string name, string pass)
        {
            using (var dbContext = new GameDbContext())
            {
                var dbPlayer = await dbContext.Players.FirstOrDefaultAsync(o => o.Name.ToLower().Trim() == name.ToLower().Trim());
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

                return (IPlayer)new BasicPlayer() { Name = dbPlayer.Name };
            }
        }


        public async Task<bool> NewPlayerAsync(IPlayer player, string pass)
        {
            try
            {

                var dbPlayer = new DbPlayer()
                {
                    RecordBy = "notlinktoausercontextyet",
                    RecordDate = DateTimeOffset.Now,
                    Name = player.Name,
                    Password = PasswordStorage.CreateHash(pass)
                };
                using (var context = new GameDbContext())
                {
                    context.Players.Add(dbPlayer);
                    await context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                _logger.LogWarning("new player rejected: " + ex.Message);
                return false;
            }

        }

        public async Task UpdatePlayerAsync(IPlayer player)
        {
            using (var context = new GameDbContext())
            {
                var dbPlayer = await context.Players.FirstOrDefaultAsync();

                dbPlayer.Name = player.Name;
                dbPlayer.LastMap = player.Map.Id;

                context.Players.Update(dbPlayer);
                await context.SaveChangesAsync();
            }

        }

        public Task UpdatePlayersAsync(IEnumerable<IPlayer> players)
        {
            throw new NotImplementedException();
        }

        public async Task NewItemAsync(IItem item)
        {
            // use some sort of mapper? to clean this up?
            var gridItem = (GridItem)item;
            var dbItem = new DbItem()
            {
                Name = item.Name,
                Description = item.Description,
                ItemTypes = gridItem.ItemTypes.Select(it => new DbItemTypes() { ItemType = (byte)it }).ToList()
            };

            using (var context = new GameDbContext())
            {
                await context.Items.AddAsync(dbItem);
                await context.SaveChangesAsync();
            }

        }

        public async Task UpdateItemAsync(IItem item)
        {
            using (var context = new GameDbContext())
            {
                var dbItem = await context.Items.Include(i => i.ItemTypes).FirstOrDefaultAsync();
                if (dbItem == null)
                    return;

                var gridItem = (GridItem)item;

                dbItem.Name = gridItem.Name;
                dbItem.Description = gridItem.Description;

                if (dbItem.ItemTypes.Any())
                {
                    dbItem.ItemTypes.Clear();
                    dbItem = context.Items.Update(dbItem).Entity;
                }

                dbItem.ItemTypes = gridItem.ItemTypes.Select(it => new DbItemTypes() { ItemType = (byte)it }).ToList();
                context.Items.Update(dbItem);
                await context.SaveChangesAsync();

            }
        }

        public async Task DelteItemAsync(IItem item)
        {
            using (var context = new GameDbContext())
            {
                var dbItem = await context.Items.FirstOrDefaultAsync(i => i.Id == item.Id);
                if (dbItem == null)
                    return;

                context.Items.Remove(dbItem);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<IItem>> LoadItemsAsync()
        {
            using (var dbContext = new GameDbContext())
            {
                var items = await dbContext.Items.Select(db =>
                new GridItem(
                    db.Id, db.Name, db.Description,
                    db.ItemTypes.Select(it => (ItemTypes)it.ItemType).ToList()
                    )).ToListAsync();

                _logger.LogInformation("Loaded items");
                return items;
            }
        }

        public async Task<IEnumerable<ISpawner>> LoadSpawnersAsync()
        {
            using (var dbContext = new GameDbContext())
            {
                var dbSpawners = await dbContext.Spawners.ToListAsync();

                var result = new List<ISpawner>();
                foreach (var dbSpawner in dbSpawners)
                {
                    var spawnType = (SpawnType)dbSpawner.SpawnType;
                    switch (spawnType)
                    {
                        case SpawnType.Item:
                            result.Add((ISpawner)new GridItemSpawner(dbSpawner.MapId, dbSpawner.EntityId));
                            break;
                        default:
                            _logger.LogWarning($"Unsupported spawn type from database was ignored SpawnerId: {dbSpawner.SpawnType}");
                            break;
                    }
                }

                _logger.LogInformation("Loaded spawners");
                return result;
            }
        }

        public async Task NewSpawnerAsync(ISpawner spawner)
        {
            var sSpawner = (SingletonSpawner)spawner;

            // use some sort of mapper? to clean this up?
            var dbSpawner = new DbSpawner()
            {
                Enabled = true,
                EntityId = sSpawner.EntityId
            };

            using (var context = new GameDbContext())
            {
                await context.Spawners.AddAsync(dbSpawner);
                await context.SaveChangesAsync();
            }
        }

        public Task UpdateSpawnerAsync(ISpawner spawner)
        {
            throw new NotImplementedException();
        }

        public Task DelteSpawnerAsync(ISpawner spawner)
        {
            throw new NotImplementedException();
        }
    }
}
