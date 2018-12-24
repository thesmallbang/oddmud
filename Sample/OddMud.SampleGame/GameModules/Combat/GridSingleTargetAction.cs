using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.SampleGame.Extensions;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules.Combat
{
    public class GridSingleTargetAction : GridAction
    {

    

        public GridEntity TargetEntity { get; set; }
      


        public override async Task<bool> Execute()
        {

            if (TargetEntity == null)
                return false;

            if (SourceEntity.Map != TargetEntity.Map)
                return false;



            // apply modifiers
            foreach (var modifier in Modifiers)
            {
                var entityWithStat = modifier.TargetType == TargetTypes.Target ? TargetEntity : SourceEntity;

                var stat = entityWithStat.Stats.FirstOrDefault(s => s.Name == modifier.Name);
                if (stat != null)
                {

                    double applyValue = 0;
                    switch (modifier.ModifierType)
                    {
                        case ActionModifierType.Flat:
                            applyValue = modifier.Value;
                            break;
                        case ActionModifierType.Percent:
                            applyValue = ((double)(modifier.Value / (double)stat.Base) * (double)100);
                            break;
                    }

                    await stat.ApplyAsync((int)applyValue);

                    if (stat.Name == "health")
                    {
                      //  if (applyValue < 0)
                      //      DamageDone = Math.Abs((int)applyValue);

                        if (stat.Value == 0)
                            await TargetEntity.KillAsync();

                    }

                }
            }

            ExecutedTime = DateTime.Now;

            return await Executing();
        }

        public override Task SetDefaultTargetAsync(IEnumerable<GridEntity> entities)
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

        public override void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            builder
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} hits ")
                .AddText($"{TargetEntity.Name}")
             //   .AddText($"{DamageDone}", TextColor.Red)
             //   .AddTextLine(" damage")
                .EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

        public override string ToMessage()
        {
            return $"{SourceEntity?.Name} hits {TargetEntity?.Name}";
        }


    }
}
