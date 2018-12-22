using Microsoft.Extensions.Logging;
using OddMud.SampleGame;
using OddMud.SampleGame.Extensions;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.View.MudLike;
using System;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class OnGameDayChangedPlugin : TickIntervalEventPlugin
    {
        public override string Name => nameof(OnGameDayChangedPlugin);
        public override int Interval => 10 * 1000;
        private DateTime lastRunWorldDate;


        public override async Task IntervalTick(object sender, EventArgs e)
        {
            await base.IntervalTick(sender, e);

            var worldTime = ((GridWorld)Game.World).Time.WorldTime;
            if (lastRunWorldDate.Day != worldTime.Day)
            {
                lastRunWorldDate = worldTime;
                var viewUpdate = MudLikeOperationBuilder.Start("dateview")
                    .AddWorldDate(worldTime)
                    .Build();

                await Game.Network.SendViewCommandsToPlayersAsync(Game.Players, MudLikeViewBuilder.Start().AddOperation(viewUpdate).Build());
            }


        }
    }
}
