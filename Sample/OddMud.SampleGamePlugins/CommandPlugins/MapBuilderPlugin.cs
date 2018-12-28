using OddMud.Core.Interfaces;
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
        public override string Name => nameof(MapBuilderPlugin);
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

                   parsed.AddExits.ToList().ForEach((exit) =>
                   {
                       try
                       {
                           map.AddExit(Enum.Parse<Exits>(exit, ignoreCase: true));
                       }
                       catch (Exception)
                       {
                           Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "invalid exit..");
                       }

                       hasUpdate = true;
                   });

                   parsed.RemoveExits.ToList().ForEach((exit) =>
                   {
                       map.RemoveExit(Enum.Parse<Exits>(exit, ignoreCase: true));
                       hasUpdate = true;
                   });

                   if (hasUpdate)
                   {
                       await Game.Store.UpdateMapsAsync(Game, new List<IMap>() { map });

                       //var mapView = MudLikeOperationBuilder.Start("mapdata").AddMap(map).Build();

                       //await Game.Network.SendViewCommandsToMapAsync(map, MudLikeViewBuilder.Start().AddOperation(mapView).Build());

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
                    Exits parsedExit = Exits.None;
                    var gridMap = (GridMap)player.Map;

                    if (!string.IsNullOrEmpty(parsed.Direction))
                    {
                        var parsedDirection = parsed.Direction;
                        switch (parsedDirection)
                        {
                            case "n":
                            case "north":
                                parsedExit = Exits.North;
                                break;
                            case "e":
                            case "east":
                                parsedExit = Exits.East;
                                break;
                            case "s":
                            case "south":
                                parsedExit = Exits.South;
                                break;

                            case "w":
                            case "west":
                                parsedExit = Exits.West;
                                break;

                            case "u":
                            case "up":
                                parsedExit = Exits.Up;
                                break;

                            case "d":
                            case "down":
                                parsedExit = Exits.Down;
                                break;


                        }

                        if (parsedExit == Exits.None)
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


                    var newMapExits = new List<Exits>();

                    // to help assist with creation, we are going to automatically add an exit on the current map for the direction of the next map.
                    // this only occurs if the direction flag was used
                    if (parsedExit != Exits.None)
                    {
                        if (!gridMap.Exits.Contains(parsedExit))
                        {
                            Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Adding exit to current map");
                            gridMap.AddExit(parsedExit);
                            await Game.Store.UpdateMapsAsync(Game, new List<IMap>() { gridMap });

                            //var mapView = MudLikeCommandBuilder.Start()
                            //    .AddMap((GridMap)player.Map)
                            //    .AddPlayers(player.Map.Players)
                            //    .Build(ViewCommandType.Set);
                            //await Game.Network.SendViewCommandsToMapAsync(gridMap, mapView);

                        }

                        // automatically add an exit back on the new map
                        switch (parsedExit)
                        {
                            case Exits.North:
                                newMapExits.Add(Exits.South);
                                break;
                            case Exits.East:
                                newMapExits.Add(Exits.West);
                                break;
                            case Exits.South:
                                newMapExits.Add(Exits.North);
                                break;
                            case Exits.West:
                                newMapExits.Add(Exits.East);
                                break;
                            case Exits.Up:
                                newMapExits.Add(Exits.Down);
                                break;
                            case Exits.Down:
                                newMapExits.Add(Exits.Up);
                                break;
                        }
                    }

                    Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "saving new map");
                    var map = new GridMap(0, string.Join(" ", parsed.Name), string.Join(" ", parsed.Description), newMapLocation, newMapExits);
                    map = (GridMap)await Game.Store.NewMapAsync(Game, (IMap)map);
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
                                    case Exits.North:
                                        cleanupMap.RemoveExit(Exits.South);
                                        break;
                                    case Exits.East:
                                        cleanupMap.RemoveExit(Exits.West);
                                        break;
                                    case Exits.South:
                                        cleanupMap.RemoveExit(Exits.North);
                                        break;
                                    case Exits.West:
                                        cleanupMap.RemoveExit(Exits.East);
                                        break;
                                    case Exits.Up:
                                        cleanupMap.RemoveExit(Exits.Down);
                                        break;
                                    case Exits.Down:
                                        cleanupMap.RemoveExit(Exits.Up);
                                        break;

                                }

                                await Game.Store.UpdateMapsAsync(Game, new List<IMap>() { cleanupMap });

                            }

                        }

                        await Game.World.RemoveMapAsync(map);
                        await Game.Store.DeleteMapsAsync(Game, new List<IMap>() { map });
                        await Game.Network.SendMessageToPlayerAsync(player, $"Removed map. HadExits[{map.Exits.Count > 0}]");

                    }


                })
                .WithNotParsed(async (issues) =>
                {
                    await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type map help");
                });


            return Task.CompletedTask;
        }


        private GridLocation GetNextLocation(GridLocation currentGridLocation, Exits exit)
        {
            switch (exit)
            {
                case Exits.North:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y - 1, currentGridLocation.Z);
                case Exits.East:
                    return new GridLocation(currentGridLocation.X + 1, currentGridLocation.Y, currentGridLocation.Z);
                case Exits.South:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y + 1, currentGridLocation.Z);
                case Exits.West:
                    return new GridLocation(currentGridLocation.X - 1, currentGridLocation.Y, currentGridLocation.Z);
                case Exits.Up:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y, currentGridLocation.Z + 1);
                case Exits.Down:
                    return new GridLocation(currentGridLocation.X, currentGridLocation.Y, currentGridLocation.Z - 1);

            }

            return null;
        }


    }
}
