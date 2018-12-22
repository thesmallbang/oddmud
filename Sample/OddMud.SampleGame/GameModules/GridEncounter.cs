
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
        public event Func<IEncounter, EncounterEndings, Task> Ended;
        private bool _started;


        public GridEncounter(int id)
        {
            Id = id;
        
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

            if (!_started)
                await Start(game);

            // any pending actions from either combatant?
            foreach (var entity in Combatants.Keys.Select(o => o).ToList())
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

                    var actionView = MudLikeOperationBuilder.Start(ViewOperationType.Append, $"enc_{Id}");

                    nextAction.AppendToOperation(actionView);


                    await game.Network.SendViewCommandsToMapAsync(entity.Map, MudLikeViewBuilder.Start().AddOperation(actionView.Build()).Build());
                }
                else
                {
                }

            }

        }

        private Task Start(IGame game)
        {
            _started = true;
            var actionView = MudLikeOperationBuilder.Start(ViewOperationType.Set, $"enc_{Id}")
                      .StartContainer($"enc_{Id}")
                      .EndContainer($"enc_{Id}")
                      .Build();

            return game.Network.SendViewCommandsToMapAsync(Combatants.FirstOrDefault().Key.Map, MudLikeViewBuilder.Start().AddOperation(actionView).Build());


        }


    }
}
