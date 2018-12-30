using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.Core.Plugins;
using OddMud.SampleGame.GameModules.Combat;
using OddMud.SampleGame.ViewComponents;
using OddMud.View.ComponentBased;
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

                    var playerData = new PlayerData() { Id = entity.Id, Name = entity.Name };
                    var changed = false;
                    

                    var vitals = new List<string>() { "health", "mana", "stamina" };
                    entity.Stats.Where(s => s.Value > 0 && s.Value < s.Base && vitals.Contains(s.Name)).ToList().ForEach(async vital =>
                    {
                        var percent = (1 / (double)vital.Base) * 100;
                        await vital.ApplyAsync((int)percent);

                        var currentStatPercent = Convert.ToInt32(((double)vital.Value / (double)vital.Base * 100));
                        changed = true;
                        switch (vital.Name)
                        {
                            case "health":
                                playerData.Health = currentStatPercent;
                                break;
                            case "mana":
                                playerData.Mana = currentStatPercent;
                                break;
                            case "stamina":
                                playerData.Stamina = currentStatPercent;
                                break;
                        }
                      
                    });

                    if (changed)
                    {
                        var statUpdate = ComponentViewBuilder<ComponentTypes>.Start().AddComponent(ComponentTypes.PlayerData, playerData);
                        await Game.Network.SendViewCommandsToPlayerAsync(entity, statUpdate);
                    }


                }


            });

            await base.IntervalTick(sender, e);
        }

    }
}
