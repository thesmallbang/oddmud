using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame;
using OddMud.SampleGame.GameModules;
using OddMud.SampleGame.GameModules.Combat;
using OddMud.SampleGame.GameModules.Combat.Intelligence;
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

            var actions = dbPlayer.Class.Actions.Select(aref => aref.Action).Select(action => new GridTargetAction()
            {
                Name = action.Name,
                TargetType = action.TargetType,
                Id = action.Id,
                Modifiers = action.Modifiers.Select(m => (IActionModifier)new GridActionModifier() { Name = m.Name, TargetType = m.TargetType, ModifierType = m.ModifierType, Min = m.Min, Max = m.Max }).ToList(),
                Element = new Element() { Id = action.ElementType.Id, Name = action.ElementType.Name, TextColor = action.ElementType.TextColor, Ranges = action.ElementType.Ranges.Select(r => (IElementRange)new ElementRange() { Text = r.Text, Min = r.Min, Max = r.Max, TextColor = r.TextColor }).ToList() }
            }).ToList();


            var player = new GridPlayer(
                    dbPlayer.Id,
                    dbPlayer.Name,
                    new List<EntityType>() { EntityType.Normal, EntityType.Combat },
                    new List<IEntityComponent>() { },
                    dbPlayer.Items.Select(dbItem => dbItem.BaseItem.ToItem(
                        dbItem.Stats.Select(s =>
                            new BasicStat(s.Name, s.Base, s.Current)).ToList()
                        )).ToList(),
                    dbPlayer.Stats.Select(s => new BasicStat(s.Name, s.Base, s.Current)).ToList(),
                    game.World.Maps.FirstOrDefault(m => m.Id == dbPlayer.LastMap)
                    );

            player.EntityTypes.ForEach(e =>
                        {
                            switch (e)
                            {
                                case EntityType.Combat:
                                    var playerIntelligence = new PlayerIntelligence(actions.FirstOrDefault());
                                    player.EntityComponents.Add(new GridCombatant() { AllowedActions = actions,   Intelligence = playerIntelligence });
                                    playerIntelligence.Configure(player);
                                    break;
                            }
                        });

            return player;

        }

        public static GridEntity ToEntity(this DbEntity dbEntity, IGame game)
        {
            var entityTypes = dbEntity.EntityTypes.Select(et => et.EntityType).ToList();


            IEncounterIntelligence intelligence = null;


            var components = new List<IEntityComponent>();
            entityTypes.ForEach(e =>
            {
                switch (e)
                {
                    case EntityType.Combat:
             
                        var actions = dbEntity.Class.Actions.Select(aref => aref.Action).Select(action => new GridTargetAction()
                        {
                            Name = action.Name,
                            TargetType = action.TargetType,
                            Id = action.Id,
                            Modifiers = action.Modifiers.Select(m => (IActionModifier)new GridActionModifier() { Name = m.Name, TargetType = m.TargetType, ModifierType = m.ModifierType, Min = m.Min, Max = m.Max }).ToList(),
                            Element = new Element() { Id = action.ElementType.Id, Name = action.ElementType.Name, TextColor = action.ElementType.TextColor, Ranges = action.ElementType.Ranges.Select(r => (IElementRange)new ElementRange() { Text = r.Text, Min = r.Min, Max = r.Max, TextColor = r.TextColor }).ToList() }
                        }).ToList();


                        switch (dbEntity.ClassId.GetValueOrDefault(0))
                        {
                            case 1:
                                intelligence = new GenericEntityIntelligence(actions.Select(a => (ICombatAction)a).ToList(), actions.FirstOrDefault());
                                break;
                            default:
                                intelligence = new PlayerIntelligence(actions.FirstOrDefault());
                                break;
                        }

                        components.Add(new GridCombatant() { AllowedActions = actions,  Intelligence = intelligence });
                        break;
                }
            });


            var entity = new GridEntity(
                    dbEntity.Id,
                    dbEntity.Name,
                    entityTypes,
                    components,
                    dbEntity.Items.Select(dbItem => dbItem.BaseItem.ToItem()).ToList(),
                    dbEntity.Stats.Select(s => new BasicStat(s.Name, s.Base, s.Current)).ToList()
                    );

            if (intelligence != null)
                intelligence.Configure(entity);



            return entity;
        }

        public static GridSpawner ToSpawner(this DbSpawner dbSpawner)
        {
            var spawnType = (SpawnType)dbSpawner.SpawnType;
            switch (spawnType)
            {
                case SpawnType.Item:
                    return new GridItemSpawner(dbSpawner.MapId, dbSpawner.EntityId);
                case SpawnType.Entity:
                    return new GridEntitySpawner(dbSpawner.MapId, dbSpawner.EntityId);
                default:
                    return null;

            }
        }

        public static GridItem ToItem(this DbItem dbItem, IEnumerable<BasicStat> overrideStats = null)
        {
            return new GridItem(
                   dbItem.Id, dbItem.Name, dbItem.Description,
                   dbItem.ItemTypes.Select(it => it.ItemType).ToList(),
                   overrideStats != null ? overrideStats.ToList() : dbItem.Stats.Select(istat => new BasicStat(istat.Name, istat.Base, istat.Current)).ToList()
                   );
        }

    }
}
