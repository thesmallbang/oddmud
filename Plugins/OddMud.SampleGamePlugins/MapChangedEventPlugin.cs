using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins
{
    public class MapChangedEventPlugin : IEventPlugin
    {
        public string Name => nameof(MapChangedEventPlugin);
        public BasicGame.Game Game;

        public void Configure(IGame game)
        {
            Game = (BasicGame.Game)game;
            Game.World.MapChanged += HandleMapChanged;
        }

        private async Task HandleMapChanged(Object sender, IMapChangeEvent e)
        {
            Game.Log(LogLevel.Information, $"Map changed event moved to {e.NewMap.Name}");

            if (e.OldMap != null)
            {
                await Game.Network.RemovePlayerFromMapGroupAsync(e.Player, e.NewMap);
                Game.Network.SendMessageToMap(e.OldMap, $"{e.Player.Name} has left the area.");
            }

            await Game.Network.AddPlayerToMapGroupAsync(e.Player, e.NewMap);
            Game.Network.SendMessageToMapExcept(e.NewMap, e.Player, $"{e.Player.Name} has joined the area.");



        }

    }
}
