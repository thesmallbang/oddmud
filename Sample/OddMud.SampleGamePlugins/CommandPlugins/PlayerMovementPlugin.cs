﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OddMud.SampleGame;
using OddMud.SampleGame.Commands;
using OddMud.SampleGame.Misc;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame.GameModules;
using OddMud.SampleGame.GameModules.Combat;

namespace OddMud.SampleGamePlugins.CommandPlugins
{
    public class PlayerMovementPlugin : LoggedInCommandPlugin
    {

        public override string Name => nameof(PlayerMovementPlugin);
        public new GridGame Game => (GridGame)base.Game;

        private ILogger<PlayerMovementPlugin> _logger;
        private CombatModule _combatModule;

        public override IReadOnlyList<string> Handles => _handles;
        private List<string> _handles = new List<string>() { "north", "east", "south", "west", "n", "e", "s", "w", "up", "down", "u", "d" };

        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
            _logger = (ILogger<PlayerMovementPlugin>)serviceProvider.GetService(typeof(ILogger<PlayerMovementPlugin>));
            _combatModule = (CombatModule)serviceProvider.GetService(typeof(IGameModule<CombatModule>));
        }

        public override async Task LoggedInProcessAsync(IProcessorData<CommandModel> request, IPlayer player)
        {

            var currentMap = (GridMap)player.Map;
            var currentGridLocation = currentMap.Location;

            var exit = Exits.None;

            switch (request.Data.FirstPart)
            {
                case "n":
                case "north":
                    exit = Exits.North;
                    break;
                case "e":
                case "east":
                    exit = Exits.East;
                    break;
                case "s":
                case "south":
                    exit = Exits.South;
                    break;
                case "w":
                case "west":
                    exit = Exits.West;
                    break;
                case "u":
                case "up":
                    exit = Exits.Up;
                    break;
                case "d":
                case "down":
                    exit = Exits.Down;
                    break;
                default:
                    return;
            }

            if (!currentMap.Exits.Any(e => e == exit))
            {
                await Game.Network.SendMessageToPlayerAsync(player, "Invalid exit");
                return;
            }

            request.Handled = true;
            var nextLocation = GetNextLocation(currentGridLocation, exit);
            var nextMap = Game.World.Maps.FirstOrDefault(map => map.Location.X == nextLocation.X && map.Location.Y == nextLocation.Y && map.Location.Z == nextLocation.Z);

            if (nextMap == null)
            {
                Game.Log(LogLevel.Information, $"next map not found {nextLocation}");
                return;
            }

            await Game.World.MovePlayerAsync(player, nextMap);
            await base.LoggedInProcessAsync(request, player);
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
