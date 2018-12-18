using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OddMud.BasicGame;
using OddMud.Core.Interfaces;
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

    
        public async Task SaveMapAsync(IMap map)
        {
            try
            {

                if (map == null)
                {
                    _logger.LogWarning("null map attempted save");
                    return;
                }

                using (var dbContext = new GameDbContext())
                {

                    // new or update?
                    var dbMap = map.Id == 0 ? null : await dbContext.Maps.FirstOrDefaultAsync(m => m.Id == map.Id);
                    if (dbMap == null)
                        await SaveNewMapAsync(dbContext, (GridMap)map);
                    else
                        await UpdateMapAsync(dbContext, dbMap, (GridMap)map);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }


        }

        private async Task UpdateMapAsync(GameDbContext context, Map dbMap, GridMap map)
        {
            dbMap.Name = map.Name;
            dbMap.Description = map.Description;
            dbMap.LocationX = map.Location.X;
            dbMap.LocationY = map.Location.Y;
            dbMap.LocationZ = map.Location.Z;
            dbMap.ModifiedBy = "nousercontextyet";
            dbMap.ModifiedDate = DateTime.Now;

            context.Maps.Update(dbMap);
            await context.SaveChangesAsync();

        }

        private async Task SaveNewMapAsync(GameDbContext context, GridMap map)
        {
            var gridMap = (GridMap)map;
            var dbMap = new Map()
            {
                Name = map.Name,
                Description = map.Description,
                RecordBy = "notlinktoausercontextyet",
                RecordDate = DateTimeOffset.Now,
                Exits = gridMap.Exits.Select(exit => new MapExit() { Direction = (byte)exit }).ToList()
            };

            await context.AddAsync(dbMap);
            await context.SaveChangesAsync();
        }


        public Task<IEnumerable<IPlayer>> LoadPlayersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IMap>> LoadMapsAsync()
        {

            using (var dbContext = new GameDbContext())
            {
                return await dbContext.Maps.Select(db =>
                new GridMap(
                    db.Id, db.Name, db.Description, new GridLocation(db.LocationX, db.LocationY, db.LocationZ)
                    )).ToListAsync();
            }
        }

        public Task SavePlayerAsync(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public Task SavePlayersAsync(IEnumerable<IPlayer> players)
        {
            throw new NotImplementedException();
        }
    }
}
