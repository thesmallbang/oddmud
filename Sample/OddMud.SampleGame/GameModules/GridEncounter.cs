
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules
{
    public class GridEncounter : IEncounter
    {
        public Dictionary<IEntity, ICombatant> Combatants { get; } = new Dictionary<IEntity, ICombatant>();

        public int Id { get; }
        public List<ICombatAction> ActionLog { get; set; } = new List<ICombatAction>();
        public List<IEntity> Dead { get; } = new List<IEntity>();

        public event Func<IEncounter, EncounterEndings, Task> Ended;
        public event Func<IEncounter, ICombatAction, Task> ActionExecuted;

        private bool _ended;


        public GridEncounter(int id)
        {
            Id = id;

        }

        private async Task Combatant_Death(IEntity deadCombatant)
        {
            deadCombatant.Died -= Combatant_Death;

            Dead.Add(deadCombatant);

            Debug.WriteLine("Combatant Died " + deadCombatant.Name);


            _ended = true;

            var aliveNonPlayers = Combatants.Count(c => typeof(GridPlayer) != c.Key.GetType());
            //if (deadNonPlayers == 0)

            foreach (var combatant in Combatants.Keys.ToList())
            {
                combatant.Died -= Combatant_Death;
            }

            // expand here when ready to support more than 2 entities in an encounter
            if (Ended != null)
                await Ended.Invoke(this, EncounterEndings.Death);



        }
        public Task AddCombatantAsync(GridEntity entity, ICombatant combatant)
        {
            Combatants.Add(entity, combatant);
            entity.Died += Combatant_Death;
            return Task.CompletedTask;
        }


        public Task TerminateAsync()
        {
            if (Ended != null)
                Ended.Invoke(this, EncounterEndings.Other);

            return Task.CompletedTask;
        }

        public async Task TickAsync(IGame game)
        {

            if (_ended)
                return;

            // any pending actions from not dead combatant?
            foreach (var entity in Combatants.Keys.Where(k => !Dead.Contains(k)).Select(o => o).ToList())
            {

                var combatant = (ICombatant<ICombatAction<GridEntity>>)Combatants[entity];
                if (combatant.CanAttack)
                {
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

                    ActionLog.Add(nextAction);

                    if (ActionExecuted != null)
                        await ActionExecuted.Invoke(this, nextAction);

                }
            }

        }

       

    }
}
