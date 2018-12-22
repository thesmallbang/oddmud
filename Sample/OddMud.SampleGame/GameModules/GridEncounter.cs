
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public event Func<IEncounter, EncounterEndings, Task> Ended;


        public GridEncounter(int id, Dictionary<IEntity, ICombatant> combatants)
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

            Debug.WriteLine("Combatant Died");

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
            foreach (var entity in Combatants.Keys.Select(o => o).ToList())
            {

                var combatant = (ICombatant<ICombatAction<GridEntity>>)Combatants[entity];
                if (combatant.CanAttack)
                {
                    Debug.WriteLine($"Can Attack True.. {entity.Name}");
                    var nextAction = await combatant.GetNextActionAsync();
                    if (nextAction == null)
                        continue;

                    if (nextAction.SourceEntity == null)
                        nextAction.SourceEntity = (GridEntity)entity;

                    if (nextAction.TargetEntity == null)
                    {

                        if (nextAction.TargetEntity == null)
                            await nextAction.SetDefaultTargetAsync(Combatants.Keys.Select(k => (GridEntity)k).ToList());

                    }

                    await nextAction.Execute();
                    Debug.WriteLine("Executed");
                    await game.Network.SendViewCommandsToMapAsync(entity.Map, nextAction.ToView());
                    await game.Network.SendMessageToMapAsync(entity.Map, nextAction.ToMessage());

                } else
                {
                }

            }

        }
    }
}
