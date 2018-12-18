using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.BasicGame;
using OddMud.BasicGame.Commands;
using OddMud.BasicGame.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.SampleGamePlugins;
using CommandLine;
using OddMud.BasicGame.Misc;
using System.Linq;

namespace OddMud.BasicGamePlugins.CommandPlugins
{


    public class CreateMapParserOptions
    {
        [Option('n', "name", Required = true, HelpText = "Name of the map.")]
        public  IEnumerable<string> Name { get; set; }

        [Option('d', "description", Required = true, HelpText = "Description of the map.")]
        public IEnumerable<string> Description { get; set; }

        [Option(longName: "direction", Required = false, HelpText = "set the location in a direction from the player insteadd of the location coordinates")]
        public string Direction { get; set; }

        [Option('x', "xlocation", Required = false, HelpText = "set the X coordinate")]
        public int X { get; set; }

        [Option('y', "ylocation", Required = false, HelpText = "set the Y coordinate")]
        public int Y { get; set; }

        [Option('z', "zlocation", Required = false, HelpText = "set the Z coordinate")]
        public int Z { get; set; }



    }
    public class DeleteMapParserOptions
    {

        [Option(longName: "direction", Required = false, HelpText = "direction of the map to delete")]
        public string Direction { get; set; }

        [Option('x', "xlocation", Required = false, HelpText = "coordinates for map to delete")]
        public int X { get; set; }

        [Option('y', "ylocation", Required = false, HelpText = "coordinates for map to delete")]
        public int Y { get; set; }

        [Option('z', "zlocation", Required = false, HelpText = "coordinates for map to delete")]
        public int Z { get; set; }



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

            }

            return base.LoggedInProcessAsync(request, player);
        }


        private Task ProcessCreateAsync(IProcessorData<CommandModel> request, IPlayer player)
        {
            request.Handled = true;
            
            Parser.Default.ParseArguments<CreateMapParserOptions>(request.Data.StringFrom(2).Split(' '))
                .WithParsed(async (parsed) =>
                {
                    if (string.IsNullOrEmpty(parsed.Direction) && parsed.X == 0 && parsed.Y == 0 && parsed.Z == 0)
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
                        newMapLocation = new GridLocation(parsed.X, parsed.Y, parsed.Z);
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
                    await Game.Store.NewMapAsync(map);
                    Game.Log(Microsoft.Extensions.Logging.LogLevel.Information, "adding to active world");
                    Game.World.AddMap(map);

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
                    if (string.IsNullOrEmpty(parsed.Direction) && parsed.X == 0 && parsed.Y == 0 && parsed.Z == 0)
                    {
                        await Game.Network.SendMessageToPlayerAsync(player, "direction or location parameters are required");
                        return;
                    }


                })
                .WithNotParsed(async (issues) =>
                {
                    await Game.Network.SendMessageToPlayerAsync(player, "invalid command - for help type map help");
                });


            return Task.CompletedTask;
        }


        private GridLocation GetNextLocation(GridLocation currentGridLocation, BasicGame.Misc.GridExits exit)
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
