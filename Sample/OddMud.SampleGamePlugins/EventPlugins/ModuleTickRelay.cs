using OddMud.SampleGame;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OddMud.SampleGame.GameModules;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class ModuleTickRelay : TickIntervalEventPlugin
    {
        public override string Name => nameof(ModuleTickRelay);
        private List<IGameModule> gameModules = new List<IGameModule>();



        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            base.Configure(game, serviceProvider);
            gameModules.AddRange((IEnumerable<IGameModule>)serviceProvider.GetServices(typeof(IGameModule<CombatModule>)));
        }

        public override async Task IntervalTick(object sender, EventArgs e)
        {
            foreach (var module in gameModules)
            {
                await module.TickAsync();
            }
            await base.IntervalTick(sender, e);

        }

    }
}
