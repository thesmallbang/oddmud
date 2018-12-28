
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Game;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;

namespace OddMud.SampleGame.GameModules.Combat
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
        public DateTime LastAction = DateTime.Now;

        public Dictionary<string, List<IEntity>> Factions { get; } = new Dictionary<string, List<IEntity>>();



        public GridEncounter(int id)
        {
            Id = id;
            Factions.Add("faction_1", new List<IEntity>());
            Factions.Add("faction_2", new List<IEntity>());

        }


        private async Task Combatant_Death(IEntity deadCombatant)
        {
            deadCombatant.Died -= Combatant_Death;

            Dead.Add(deadCombatant);

            Debug.WriteLine("Combatant Died " + deadCombatant.Name);


            // figure out how many are alive per faction

            foreach (var faction in Factions.Keys.ToList())
            {
                // if a faction has no members alive then we are ending
                var aliveCount = Factions[faction].Count(e => !Dead.Contains(e));
                if (aliveCount == 0)
                {
                    _ended = true;
                    foreach (var combatant in Combatants.Keys.ToList())
                    {
                        combatant.Died -= Combatant_Death;
                    }


                    if (Ended != null)
                        await Ended.Invoke(this, EncounterEndings.Death);

                    //break;
                    return;
                }
            }


        }
        public Task AddCombatantAsync(GridEntity entity, ICombatant combatant, string factionKey = "")
        {

            List<IEntity> faction;
                   
            if (entity.IsPlayer())
            {
                Dead.RemoveAll(d => d.Id == entity.Id);
            }

            // some default behaviour will be to find the faction with the matching isPlayer
            if (string.IsNullOrEmpty(factionKey))
            {
                var testFaction = Factions.FirstOrDefault(o => o.Value.Any(e => e.IsPlayer() == entity.IsPlayer()));
                if (testFaction.Value == null)
                    testFaction = Factions.FirstOrDefault(o => o.Value.Count == 0);
                if (testFaction.Value == null)
                    testFaction = Factions.FirstOrDefault();

                faction = testFaction.Value;
            }
            else
            {
                faction = Factions[factionKey];

            }
            if (faction != null)
                faction.Add(entity);

            Combatants.Add(entity, combatant);


            entity.Died += Combatant_Death;
            return Task.CompletedTask;
        }


        public Task TerminateAsync(EncounterEndings ending = EncounterEndings.Other)
        {

            foreach (var combatant in Combatants.Keys.ToList())
            {
                combatant.Died -= Combatant_Death;
            }

            if (Ended != null)
                Ended.Invoke(this, ending);

            return Task.CompletedTask;
        }

        public async Task TickAsync(IGame game)
        {

            if (_ended)
                return;


            // if no factions are on the same map then pause operations
            var maps = Combatants.Keys.Select(c => c.Map.Id).Distinct();

            if (maps.Count() == Combatants.Keys.Count())
            {
                // no combatants are on the same map... skip
                return;
            }

            // any pending actions from not dead combatant?
            var entities = Combatants.Keys.Where(k => !Dead.Contains(k)).Select(o => o).ToList();
            foreach (var entity in entities)
            {
                // player offline?
                if (entity.IsPlayer() && !game.Players.Contains((IPlayer)entity))
                {


                    // are there any players left? pause combat
                    // i dont like making the specific decision about no offline combat here..
                    // needs configuration in future
                    if (Combatants.Keys.Count(k => k.IsPlayer() && game.Players.Contains((IPlayer)k)) == 0)
                    {
                        return;
                    }


                }

                var combatant = (ICombatant<GridTargetAction>)Combatants[entity];
                if (combatant.CanAttack)
                {
                    var nextAction = await combatant.GetNextActionAsync(this);
                    if (nextAction == null)
                        continue;

                    if (nextAction.SourceEntity == null)
                        nextAction.SourceEntity = (GridEntity)entity;

                    if (nextAction.TargetEntities.Count == 0)
                    {
                        await nextAction.SetDefaultTargetAsync(this);
                    }

                    var executed = await nextAction.Execute();

                    if (executed)
                    {
                        LastAction = DateTime.Now;
                        ActionLog.Add(nextAction);

                        if (ActionExecuted != null)
                            await ActionExecuted.Invoke(this, nextAction);
                    }
                }
            }

        }



    }
}
