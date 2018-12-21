using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame;
using OddMud.SampleGame.Misc;
using OddMud.Web.Game.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database
{
    public static class DbConverters
    {
        public static GridMap ToMap(this DbMap db)
        {
            return new GridMap(
                    db.Id, db.Name, db.Description,
                    new GridLocation(db.LocationX, db.LocationY, db.LocationZ),
                    db.Exits.Select(e => (Exits)e.Direction).ToList()
                    );
        }

        public static GridPlayer ToPlayer(this DbPlayer dbPlayer, IGame game)
        {
            return new GridPlayer(
                    dbPlayer.Id,
                    dbPlayer.Name,
                    (EntityClasses)dbPlayer.Class,
                    game.World.Maps.FirstOrDefault(m => m.Id == dbPlayer.LastMap),
                    dbPlayer.Items.Select(dbItem => dbItem.BaseItem.ToItem(
                        dbItem.Stats.Select(s =>
                            new BasicStat(s.Name, s.Base, s.Current)).ToList()
                        )).ToList()
                    );
        }

        public static GridSpawner ToSpawner(this DbSpawner dbSpawner)
        {
            var spawnType = (SpawnType)dbSpawner.SpawnType;
            switch (spawnType)
            {
                case SpawnType.Item:
                    return new GridItemSpawner(dbSpawner.MapId, dbSpawner.EntityId);
                default:
                    return null;

            }
        }

        public static GridItem ToItem(this DbItem dbItem, IEnumerable<BasicStat> overrideStats = null)
        {
            return new GridItem(
                   dbItem.Id, dbItem.Name, dbItem.Description,
                   dbItem.ItemTypes.Select(it => (ItemTypes)it.ItemType).ToList(),
                   overrideStats != null ? overrideStats.ToList() : dbItem.Stats.Select(istat => new BasicStat(istat.Name, istat.Base, istat.Current)).ToList()
                   );
        }

    }
}
