﻿using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.SampleGamePlugins;
using CommandLine;
using OddMud.SampleGame.Misc;
using System.Linq;
using OddMud.View.MudLike;

namespace OddMud.SampleGamePlugins.CommandPlugins
{


    public class CreateMapParserOptions
    {
        [Option('n', "name", Required = true, HelpText = "Name of the map.")]
        public IEnumerable<string> Name { get; set; }

        [Option('d', "description", Required = true, HelpText = "Description of the map.")]
        public IEnumerable<string> Description { get; set; }

        [Option(longName: "direction", Required = false, HelpText = "set the location in a direction from the player insteadd of the location coordinates")]
        public string Direction { get; set; }

        [Option('x', "xlocation", Required = false, HelpText = "set the X coordinate")]
        public int? X { get; set; }

        [Option('y', "ylocation", Required = false, HelpText = "set the Y coordinate")]
        public int? Y { get; set; }

        [Option('z', "zlocation", Required = false, HelpText = "set the Z coordinate")]
        public int? Z { get; set; }

    }

    public class EditMapParserOptions
    {


        [Option(longName: "id", Required = false, HelpText = "id of the map to edit")]
        public int? Id { get; set; }


        [Option('n', "name", Required = false, HelpText = "Name of the map.")]
        public IEnumerable<string> Name { get; set; }

        [Option('d', "description", Required = false, HelpText = "Description of the map.")]
        public IEnumerable<string> Description { get; set; }


        [Option('a', "addexits", Required = false)]
        public IEnumerable<string> AddExits { get; set; }

        [Option('r', "removeexits", Required = false)]
        public IEnumerable<string> RemoveExits { get; set; }

        [Option('c', "cleanupexits", Required = false, HelpText = "cleanup remote exits on remove", Default = true)]
        public bool Cleanup { get; set; }

    }

    public class DeleteMapParserOptions
    {

        [Option(longName: "id", Required = true, HelpText = "id of the map to delete")]
        public int? Id { get; set; }


        [Option('c', "cleanup", Required = false, HelpText = "cleanup exits for surrounding maps", Default = true)]
        public bool Cleanup { get; set; }


    }

    public class MapBuilderPlugin : LoggedInCommandPlugin
    {
        public override string Name => nameof(ChatPlugin);
        public new GridGame Game => (GridGame)base.Game;
        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "map" };

        public override Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            switch (request.Data.SecondPart)
            {
                case "create":
                    return ProcessCreateAsync(request, player);
                case "delete":
                    return ProcessDeleteAsync(request, player);
                case "edit":
                    return ProcessEditAsync(request, player);

            }

            return base.LoggedInProcessAsync(request, player);
        }

        private Task ProcessEditAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<EditMapParserOptions>(request.Data.StringFrom(2).Split(' '))
               .WithParsed(async (parsed) =>
               {

                   var mapId = parsed.Id.HasValue ? parsed.Id.Value : player.Map.Id;
                   var map = Game.World.Maps.FirstOrDefault(m => m.Id == mapId);
                   var hasUpdate = false;

                   if (parsed.Name.Any())
                   {
                       var completeName = string.Join(" ", parsed.Name);
                       if (!string.IsNullOrWhiteSpace(completeName))
                       {
                           map.Name = completeName;
                           hasUpdate = true;
                       }
                   }
                   if (parsed.Description.Any())
                   {
                       var complete = string.Join(" ", parsed.Description);
                       if (!string.IsNullOrWhiteSpace(complete))
                       {
                           map.Description = complete;
                           hasUpdate = true;
                       }
                   }

                   parsed.AddExits.ToList().ForEach((exit) => {
                       map.AddExit(Enum.Parse<GridExits>(exit,ignoreCase: true));
                       hasUpdate = true;
                   });

                   parsed.RemoveExits.ToList().ForEach((exit) => {
                       map.RemoveExit(Enum.Parse<GridExits>(exit, ignoreCase: true));
                       hasUpdate = true;
                   });

                   if (hasUpdate)
                   {
                       await Game.Store.UpdateMapAsync(map);

                       var mapView = MudLikeCommandBuilder.Start().AddMap(map).Build(ViewCommandType.Replace, "mapdata");
                       await Game.Network.SendViewCommandsToMapAsync(map, mapView);

                   }

               })
               .WithNotParsed(async (issues) =>
               {
                   await Game.Network.SendMessageToPlayerAsync(player, "invalid command(edit) - for help type map help");
               });

            return Task.CompletedTask;
        }

        private Task ProcessCreateAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<CreateMapParserOptions>(request.Data.StringFrom(2).Split(' '))
                .WithParsed(async (parsed) =>
                {
                    if (string.IsNullOrEmpty(parsed.Direction) && parsed.X == null)
                    {
                        await Game.Network.SendMessageToPlayerAsync(player, "direction or location parameters are required");
                        return;
                    }

                    GridLocation newMapLocation = null;
                    GridExits parsedExit = GridExits.None;
                    var gridMap = (GridMap)player.Map;

                    if (!string.IsNullOrEmpty(parsed.Direction))
                    {
                        var parsedDirection = parsed.Direction;
                        switch (parsedDirection)
                        {
                            case "n":
                            case "north":
                                parsedExit = GridExits.North;
                                break;
                            case "e":
                            case "east":
                                parsedExit = GridExits.East;
                                break;
                            case "s":
                            case "south":
                                parsedExit = GridExits.South;
                                break;

                            case "w":
                            case "west":
                                parsedExit = GridExits.West;
                                break;

                            case "u":
                            case "up":
                                parsedExit = GridExits.Up;
                                break;

                            case "d":
                            case "down":
                                parsedExit = GridExits.Down;
                                break;


                        }

                        if (parsedExit == GridExits.None)
                        {
                            await Game.Network.SendMessageToPlayerAsync(player, "direction was not valid");
                            return;
                        }


                        newMapLocation = GetNextLocation(gridMap.Location, parsedExit);
                    }
                    else
                    {
                        newMapLocation = new GridLocation(parsed.X.Value, parsed.Y.Value, parsed.Z.Value);
                    }

                    if (Game.World.Maps.Any(m => m.Location.X == newMapLocation.X
                    && m.Location.Y == newMapLocation.Y
                    && m.Location.Z == newMapLocation.Z))
                    {
                        await Game.Network.SendMessageToPlayerAsync(player, "a map already exists at that location");
                        return;
                    }


                    var newMapExits = new List<GridExits>();

                    // to help assist with creation, we are going to automatically add an exit on the current map for the direction of the next map.
                    // this only occurs if the direction flag was used
                    if (parsedExit != GridExits.None)
                    {
                        if (!gridMap.Exits.Contains(parsedExit))
                        {
                            Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Adding exit to current map");
                            gridMap.AddExit(parsedExit);
                            await Game.Store.UpdateMapAsync(gridMap);

                            //var mapView = MudLikeCommandBuilder.Start()
                            //    .AddMap((GridMap)player.Map)
                            //    .AddPlayers(player.Map.Players)
                            //    .Build(ViewCommandType.Set);
                            //await Game.Network.SendViewCommandsToMapAsync(gridMap, mapView);

                        }

                        // automatically add an exit back on the new map
                        switch (parsedExit)
                        {
                            case GridExits.North:
                                newMapExits.Add(GridExits.South);
                                break;
                            case GridExits.East:
                                newMapExits.Add(GridExits.West);
                                break;
                            case GridExits.South:
                                newMapExits.Add(GridExits.North);
                                break;
                            case GridExits.West:
                                newMapExits.Add(GridExits.East);
                                break;
                            case GridExits.Up:
                                newMapExits.Add(GridExits.Down);
                                break;
                            case GridExits.Down:
                                newMapExits.Add(GridExits.Up);
                                break;
                        }
                    }

                    Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "saving new map");
                    var map = new GridMap(0, string.Join(" ", parsed.Name), string.Join(" ", parsed.Description), newMapLocation, newMapExits);
                    var newMapId = await Game.Store.NewMapAsync(map);
                    map = (GridMap)await Game.Store.LoadMapAsync(newMapId);
                    Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "adding to active world");
                    await Game.World.AddMapAsync(map);

                })
                .WithNotParsed(async (issues) =>
                {
                    await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type map help");
                });

            return Task.CompletedTask;
        }

        private Task ProcessDeleteAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;

            Parser.Default.ParseArguments<DeleteMapParserOptions>(request.Data.StringFrom(2).Split(' '))
                .WithParsed(async (parsed) =>
                {
                    if (parsed.Id == null)
                    {
                        await Game.Network.SendMessageToPlayerAsync(player, "direction or location xyz parameters are required");
                        return;
                    }

                    if (parsed.Id.HasValue)
                    {
                        var map = Game.World.Maps.FirstOrDefault(m => m.Id == parsed.Id);
                        if (map == null)
                        {
                            await Game.Network.SendMessageToPlayerAsync(player, $"map with id {parsed.Id.GetValueOrDefault(0)} was not found");
                            return;
                        }

                        // update the map exits that were connected to this one.
                        if (parsed.Cleanup && map.Exits.Count > 0)
                        {
                            foreach (var exit in map.Exits)
                            {
                                var cleanupLocation = GetNextLocation(map.Location, exit);
                                var cleanupMap = Game.World.Maps.FirstOrDefault(m =>
                                       m.Location.X == cleanupLocation.X
                                       && m.Location.Y == cleanupLocation.Y
                                       && m.Location.Z == cleanupLocation.Z);
                                if (cleanupMap == null || !cleanupMap.Exits.Any(o => o == exit))
                                {
                                    break;
                                }

                                switch (exit)
                                {
                                    case GridExits.North:
                                        cleanupMap.RemoveExit(GridExits.South);
                                        break;
                                    case GridExits.East:
                                        cleanupMap.RemoveExit(GridExits.West);
                                        break;
                                    case GridExits.South:
                                        cleanupMap.RemoveExit(GridExits.North);
                                        break;
                                    case GridExits.West:
                                        cleanupMap.RemoveExit(GridExits.East);
                                        break;
                                    case GridExits.Up:
                                        cleanupMap.RemoveExit(GridExits.Down);
                                        break;
                                    case GridExits.Down:
                                        cleanupMap.RemoveExit(GridExits.Up);
                                        break;

                                }

                                await Game.Store.UpdateMapAsync(cleanupMap);

                            }

                        }

                        await Game.World.RemoveMapAsync(map);
                        await Game.Store.DeleteMapAsync(map);
                        await Game.Network.SendMessageToPlayerAsync(player, $"Removed map. HadExits[{map.Exits.Count > 0}]");

                    }


                })
                .WithNotParsed(async (issues) =>
                {
                    await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type map help");
                });


            return Task.CompletedTask;
        }


        private GridLocation GetNextLocation(GridLocation currentGridLocation, GridExits exit)
        {
            switch (exit)
            {
                case GridExits.North:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y - 1, currentGridLocation.Z);
                case GridExits.East:
                    return new GridLocation(currentGridLocation.X + 1, currentGridLocation.Y, currentGridLocation.Z);
                case GridExits.South:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y + 1, currentGridLocation.Z);
                case GridExits.West:
                    return new GridLocation(currentGridLocation.X - 1, currentGridLocation.Y, currentGridLocation.Z);
                case GridExits.Up:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y, currentGridLocation.Z + 1);
                case GridExits.Down:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y, currentGridLocation.Z - 1);

            }

            return null;
        }


    }
}
