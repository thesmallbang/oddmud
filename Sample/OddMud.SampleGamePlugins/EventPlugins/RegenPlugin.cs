using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame.GameModules.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.SampleGamePlugins.EventPlugins
{
    public class RegenPlugin : TickIntervalEventPlugin
    {

        public override string Name => nameof(RegenPlugin);

        public override int Interval => 2000;

        private CombatModule _combatModule;
        private ILogger<RegenPlugin> _logger;

        public override void Configure(IGame game, IServiceProvider serviceProvider)
        {
            _combatModule = (CombatModule)serviceProvider.GetService(typeof(IGameModule<CombatModule>));
            _logger = (ILogger<RegenPlugin>)serviceProvider.GetService(typeof(ILogger<RegenPlugin>));

            base.Configure(game, serviceProvider);
        }

        public override async Task IntervalTick(object sender, EventArgs e)
        {

            // players will just regen 1% per tick;
            // entities wont regen... i'm not cleanly tracking instances of mobs atm
            Game.Players
                .ToList().ForEach(async entity =>
            {
                if (!await _combatModule.IsInCombat(entity))
                {
                    var vitals = new List<string>() { "health", "mana", "stamina" };
                    entity.Stats.Where(s => s.Value > 0 && s.Value < s.Base && vitals.Contains(s.Name)).ToList().ForEach(async vital =>
                    {
                        var percent = (1 / (double)vital.Base) * 100;
                        await vital.ApplyAsync((int)percent);
                    });
                }
            });

            await base.IntervalTick(sender, e);
        }

    }
}
