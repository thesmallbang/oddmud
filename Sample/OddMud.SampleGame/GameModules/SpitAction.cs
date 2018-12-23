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
    public class SpitAction : ICombatAction<GridEntity>
    {

        public GridEntity SourceEntity { get; set; }

        public GridEntity TargetEntity { get; set; }
        public DateTime ExecutedTime { get; set; } 

        public int Damage { get; set; }
        private Random _randomizer = new Random();



        public async Task<bool> Execute()
        {
            ExecutedTime = DateTime.Now;

            if (TargetEntity == null)
                return false;

            if (SourceEntity.Map != TargetEntity.Map)
                return false;

            var dmg = _randomizer.Next(1, 10);
            Damage = dmg;

            var hpstat = TargetEntity.Stats.FirstOrDefault(s => s.Name == "health");
            if (hpstat == null)
                return false;

            await hpstat.ApplyAsync(-dmg);

            if (hpstat.Value == 0)
            {
                await TargetEntity.KillAsync();
            }

            return true;
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

        public void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} ")
                .AddText("spits", TextColor.Aqua)
                .AddText(" on ")
                .AddText($"{TargetEntity.Name} for ")
                .AddText($"{Damage}", TextColor.Red)
                .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

        public string ToMessage()
        {
            return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {Damage}";
        }


    }
}
