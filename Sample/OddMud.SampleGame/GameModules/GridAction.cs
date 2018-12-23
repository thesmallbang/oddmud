using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules
{
    public class GridAction : ICombatAction<GridEntity>
    {

        public GridEntity SourceEntity { get; set; }

        public GridEntity TargetEntity { get; set; }
        public DateTime ExecutedTime { get; set; } 

        public int Damage { get; set; }
        private Random _randomizer = new Random();



        public virtual Task<bool> Execute()
        {
            ExecutedTime = DateTime.Now;

            return Task.FromResult(true);
        }

        public Task SetDefaultTargetAsync(IEnumerable<GridEntity> entities)
        {
            var otherEntities = entities.Where(e => e.IsAlive && e.Map == SourceEntity.Map).Except(new List<GridEntity>() { SourceEntity }).ToList();

            if (otherEntities.Count == 0)
                return Task.CompletedTask;

            // implement some sort of hate system using combatant.Stats to alter results here..
            var isSourcePlayer = SourceEntity.IsPlayer();
            if (isSourcePlayer)
            {
                // find a monster
                var monsters = otherEntities.Where(e => !e.IsPlayer()).ToList();
                TargetEntity = monsters.FirstOrDefault();
            }
            else
            {
                // find a player
                var players = otherEntities.Where(e => e.IsPlayer()).ToList();
                TargetEntity = players.FirstOrDefault();
            }


            return Task.CompletedTask;
        }

        public virtual void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} hits ")
                .AddText($"{TargetEntity.Name} for ")
                .AddText($"{Damage}", TextColor.Red)
                .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

        public virtual string ToMessage()
        {
            return $"{SourceEntity?.Name} hits {TargetEntity?.Name} for {Damage}";
        }


    }
}
