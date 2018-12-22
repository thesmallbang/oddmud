
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public class GridEncounter : IEncounter
    {
        public Dictionary<IEntity, ICombatant> Combatants { get; } = new Dictionary<IEntity, ICombatant>();

        public int Id { get; }
        private int DeadCounter { get; set; }

        public event Func<IEncounter, EncounterEndings, Task> Ended;


        public GridEncounter(int id, Dictionary<IEntity,ICombatant> combatants)
        {
            Id = id;
            Combatants = combatants;

            foreach (var entity in combatants.Keys)
            {
                var combatant = combatants[entity];
                entity.Died += Combatant_Death;
            }

           

        }

        private async Task Combatant_Death(IEntity deadCombatant)
        {
            deadCombatant.Died -= Combatant_Death;


            //var aliveNonPlayers = Combatants.Count(c => typeof(GridPlayer) != c.Key.GetType());
            //if (deadNonPlayers == 0)

            // expand here when ready to support more than 2 entities in an encounter
            if (Ended != null)
                await Ended.Invoke(this, EncounterEndings.Death);


    
        }

        public Task TerminateAsync()
        {
            if (Ended != null)
                Ended.Invoke(this, EncounterEndings.Other);

            return Task.CompletedTask;
        }

        public async Task TickAsync(IGame game)
        {

            // any pending actions from either combatant?
            foreach (var entity in Combatants.Keys)
            {
                
                var combatant = Combatants[entity];
                if (combatant.CanAttack)
                {
                    game.Log(Microsoft.Extensions.Logging.LogLevel.Information, $"{entity.Name} is ready to attack but does nothing..");
                }

            }
        }
    }
}
