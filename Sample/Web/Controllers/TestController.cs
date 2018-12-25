using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OddMud.Web.Game.Database;
using OddMud.Web.Game.Database.Entities;

namespace OddMud.Web.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet()]
        [Route("seed")]
        public IActionResult Seed()
        {
            using (var context = new GameDbContext())
            {
                if (!context.Elements.Any())
                {
                    context.Elements.Add(new DbElement()
                    {
                        Name = "Physical",
                        TextColor = View.MudLike.TextColor.Gray,
                        Ranges = new List<DbElementRange>() {
                            new DbElementRange() { Text = "scratches", Max = 5 , TextColor = View.MudLike.TextColor.Red },
                            new DbElementRange() { Text = "hits", Max = 10 ,TextColor = View.MudLike.TextColor.Red },
                            new DbElementRange() { Text = "mauls", Max = 20 ,TextColor = View.MudLike.TextColor.Red }
                        }
                    });

                    context.Elements.Add(new DbElement()
                    {
                        Name = "Nature",
                        TextColor = View.MudLike.TextColor.Green,
                        Ranges = new List<DbElementRange>() {
                            new DbElementRange() { Text = "mends", Max = 5 , TextColor = View.MudLike.TextColor.Green },
                            new DbElementRange() { Text = "heals", Max = 20 ,TextColor = View.MudLike.TextColor.Green },
                            new DbElementRange() { Text = "GREATLY heals", Max = 100 ,TextColor = View.MudLike.TextColor.Green }
                        }
                    });

                    context.SaveChanges();
                }

                if (!context.Actions.Any())
                {
                    context.Actions.Add(new DbAction()
                    {
                        ActionType = 0,
                        ElementType = context.Elements.FirstOrDefault(element => element.Name == "Physical"),
                        Name = "punch",
                        TargetType = SampleGame.GameModules.Combat.TargetTypes.Enemy,
                        Modifiers = new List<DbActionModifier>() {
                             new DbActionModifier(){
                                 Name = "health",
                                 ModifierType = SampleGame.GameModules.Combat.ActionModifierType.Flat,
                                 TargetType = SampleGame.GameModules.Combat.ModifierTargetTypes.Other,
                                 Min = -7,
                                 Max = -3
                             }
                         }
                    });

                    context.Actions.Add(new DbAction()
                    {
                        ActionType = 0,
                        ElementType = context.Elements.FirstOrDefault(element => element.Name == "Physical"),
                        Name = "SUPER PUNCH",
                        TargetType = SampleGame.GameModules.Combat.TargetTypes.Enemy,
                        Modifiers = new List<DbActionModifier>() {
                             new DbActionModifier(){
                                 Name = "health",
                                 ModifierType = SampleGame.GameModules.Combat.ActionModifierType.Flat,
                                 TargetType = SampleGame.GameModules.Combat.ModifierTargetTypes.Other,
                                 Min = -15,
                                 Max = -5
                             },
                             new DbActionModifier(){
                                 Name = "stamina",
                                 ModifierType = SampleGame.GameModules.Combat.ActionModifierType.Flat,
                                 TargetType = SampleGame.GameModules.Combat.ModifierTargetTypes.Caster,
                                 Min = -5,
                                 Max = -5
                             }
                         }
                    });


                    context.Actions.Add(new DbAction()
                    {
                        ActionType = 0,
                        ElementType = context.Elements.FirstOrDefault(element => element.Name == "Nature"),
                        Name = "Soothe",
                        TargetType = SampleGame.GameModules.Combat.TargetTypes.Self,
                        Modifiers = new List<DbActionModifier>() {
                             new DbActionModifier(){
                                 Name = "health",
                                 ModifierType = SampleGame.GameModules.Combat.ActionModifierType.Flat,
                                 TargetType = SampleGame.GameModules.Combat.ModifierTargetTypes.Other,
                                 Min = 15,
                                 Max = 5
                             },
                             new DbActionModifier(){
                                 Name = "mana",
                                 ModifierType = SampleGame.GameModules.Combat.ActionModifierType.Flat,
                                 TargetType = SampleGame.GameModules.Combat.ModifierTargetTypes.Caster,
                                 Min = -25,
                                 Max = -25
                             }
                         }
                    });


                    context.SaveChanges();
                }


                if (!context.Classes.Any())
                {
                    context.Classes.Add(new DbClass()
                    {
                        Name = "Peasant",
                        Description = "Basic starter class",
                        Actions = new List<DbClassAction>() {
                            new DbClassAction(){ ActionId = 1 },
                            new DbClassAction(){ ActionId = 2 },
                            new DbClassAction(){ ActionId = 3 }
                        }
                    });

                    context.SaveChanges();
                }

                if (!context.Entities.Any())
                {

                    context.Entities.Add(new DbEntity()
                    {
                        ClassId = 1,
                        EntityTypes = new List<DbEntityType>() { new DbEntityType() { EntityType = Core.Game.EntityType.Normal }, new DbEntityType() { EntityType = Core.Game.EntityType.Combat } },
                        Name = "Baby Rat",
                        Stats = new List<DbEntityStat>() {
                            new DbEntityStat() { Name = "health", Base = 10, Current = 10 },
                            new DbEntityStat() { Name = "mana", Base = 1, Current = 1 },
                            new DbEntityStat() { Name = "stamina", Base = 1, Current = 1 }
                        }
                    });

                    context.Entities.Add(new DbEntity()
                    {
                        ClassId = 1,
                        EntityTypes = new List<DbEntityType>() { new DbEntityType() { EntityType = Core.Game.EntityType.Normal }, new DbEntityType() { EntityType = Core.Game.EntityType.Combat } },
                        Name = "Rat",
                        Stats = new List<DbEntityStat>() {
                            new DbEntityStat() { Name = "health", Base = 30, Current = 30 },
                            new DbEntityStat() { Name = "mana", Base = 1, Current = 1 },
                            new DbEntityStat() { Name = "stamina", Base = 10, Current = 10 }
                        },
                    });

                    context.Entities.Add(new DbEntity()
                    {
                        ClassId = 1,
                        EntityTypes = new List<DbEntityType>() { new DbEntityType() { EntityType = Core.Game.EntityType.Normal }, new DbEntityType() { EntityType = Core.Game.EntityType.Combat } },
                        Name = "Rat King",
                        Stats = new List<DbEntityStat>() {
                            new DbEntityStat() { Name = "health", Base = 50, Current = 50 },
                            new DbEntityStat() { Name = "mana", Base = 30, Current = 30 },
                            new DbEntityStat() { Name = "stamina", Base = 10, Current = 10 }
                        }
                    });
                    context.SaveChanges();
                }


                if (!context.Maps.Any())
                {
                    context.Maps.Add(new DbMap() { Name = "Sparing Area", Description = "A generic area for sparing with rats", LocationX = 0, LocationY = 0, LocationZ = 0 });

                    context.SaveChanges();
                }

                if (!context.Spawners.Any())
                {
                    context.Spawners.Add(new DbSpawner()
                    {
                        Enabled = true,
                        EntityId = 1,
                        MapId = 1,
                        SpawnType = 2
                    });
                    context.Spawners.Add(new DbSpawner()
                    {
                        Enabled = true,
                        EntityId = 2,
                        MapId = 1,
                        SpawnType = 2
                    });
                    context.Spawners.Add(new DbSpawner()
                    {
                        Enabled = true,
                        EntityId = 3,
                        MapId = 1,
                        SpawnType = 2
                    });


                    context.SaveChanges();
                }
            }
            return new OkResult();
        }
    }
}