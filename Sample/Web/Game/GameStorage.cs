using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OddMud.BasicGame;
using OddMud.BasicGame.Misc;
using OddMud.Core.Interfaces;
using OddMud.Web.Game.Database;
using OddMud.Web.Game.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                var dbMap = await context.Maps.FirstOrDefaultAsync(o => o.Id == map.Id);

                var gridMap = (GridMap)map;
                dbMap.Name = gridMap.Name;
                dbMap.Description = gridMap.Description;
                dbMap.LocationX = gridMap.Location.X;
                dbMap.LocationY = gridMap.Location.Y;
                dbMap.LocationZ = gridMap.Location.Z;
                dbMap.ModifiedBy = "nousercontextyet";
                dbMap.ModifiedDate = DateTime.Now;
                dbMap.Exits = gridMap.Exits.Select(exit => new DbMapExit() { Direction = (byte)exit }).ToList();


                context.Maps.Update(dbMap);
                await context.SaveChangesAsync();
            }
        }

        public async Task NewMapAsync(IMap map)
        {
            var gridMap = (GridMap)map;
            var dbMap = new DbMap()
            {
                Name = map.Name,
                Description = map.Description,
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Exits = gridMap.Exits.Select(exit => new DbMapExit() { Direction = (byte)exit }).ToList()
            };

            using (var context = new GameDbContext())
            {
                await context.AddAsync(dbMap);
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
                    db.Exits.Select(e => (GridExits)e.Direction).ToList()
                    )).ToListAsync();
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

                return new BasicPlayer() { Name = dbPlayer.Name };
            }
        }

      
        public async Task NewPlayerAsync(IPlayer player, string pass)
        {
            var dbPlayer = new DbPlayer() {
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Name = player.Name,
                Password = PasswordStorage.CreateHash(pass)
            };
            using (var context = new GameDbContext())
            {
                context.Players.Add(dbPlayer);
                await context.SaveChangesAsync();
            }
        }

        public Task UpdatePlayerAsync(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePlayersAsync(IEnumerable<IPlayer> players)
        {
            throw new NotImplementedException();
        }
    }
}
