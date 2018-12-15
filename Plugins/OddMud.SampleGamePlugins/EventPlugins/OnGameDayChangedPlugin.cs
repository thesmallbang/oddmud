using Microsoft.Extensions.Logging;
using OddMud.BasicGame;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Threading.Tasks;

namespace OddMud.BasicGamePlugins.EventPlugins
{
    public class OnGameDayChangedPlugin : IEventPlugin
    {
        public string Name => nameof(OnGameDayChangedPlugin);
        public BasicGame.Game Game;

        public void Configure(IGame game)
        {
            Game = (BasicGame.Game)game;
            Game.Ticked += Game_Ticked;
            
        }

        private int interval = 10;
        private DateTime lastRun;
        private DateTime lastRunWorldDate;


        private async Task Game_Ticked(object arg1, EventArgs arg2)
        {

            if (DateTime.Now.AddSeconds(-interval) >= lastRun)
            {
                var worldTime = ((BasicWorld)Game.World).Time.WorldTime;
                if (lastRunWorldDate.Day != worldTime.Day)
                {
                    await Game.Network.SendMessageToPlayersAsync(Game.Players, $"A new day has begun. [{worldTime.ToString("yyyy-MM-dd")}]");
                }
                lastRun = DateTime.Now;
                lastRunWorldDate = worldTime;
            }

            

        }
    }
}
