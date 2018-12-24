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
    public class GridTargetAction : GridAction
    {



        public List<GridEntity> TargetEntities { get; set; } = new List<GridEntity>();



        public override async Task<bool> Execute()
        {

            if (TargetEntities.Count == 0)
                return false;

            foreach (var target in TargetEntities)
            {
                // this map check will need to be moved out eventually to support things like rez
                if (SourceEntity.Map != target.Map)
                    return false;

                // apply modifiers
                foreach (var modifier in Modifiers)
                {
                    var entityWithStat = modifier.TargetType == ModifierTargetTypes.Other ? target : SourceEntity;

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
                                await target.KillAsync();

                        }

                    }
                }
            }





            ExecutedTime = DateTime.Now;

            return await Executing();
        }

        public override Task SetDefaultTargetAsync(IEncounter encounter)
        {
            var gridEncounter = (GridEncounter)encounter;

            switch (TargetType)
            {
                case TargetTypes.Self:
                    TargetEntities.Add(SourceEntity);
                    break;
                case TargetTypes.Enemy:
                    foreach (var factionName in gridEncounter.Factions.Keys.ToList())
                    {
                        var faction = gridEncounter.Factions[factionName];
                        if (!faction.Contains(SourceEntity))
                        {
                            TargetEntities.Add((GridEntity)faction.FirstOrDefault(e => !encounter.Dead.Contains(e)));
                            break;
                        }
                    }
                    break;
                case TargetTypes.EnemyArea:
                    foreach (var factionName in gridEncounter.Factions.Keys.ToList())
                    {
                        var faction = gridEncounter.Factions[factionName];
                        if (!faction.Contains(SourceEntity))
                        {
                            foreach (var target in faction)
                            {
                                TargetEntities.Add((GridEntity)target);
                            }

                        }
                    }
                    break;
                case TargetTypes.Friend:
                    foreach (var factionName in gridEncounter.Factions.Keys.ToList())
                    {
                        var faction = gridEncounter.Factions[factionName];
                        if (faction.Contains(SourceEntity))
                        {
                            TargetEntities.Add((GridEntity)faction.FirstOrDefault(e => !encounter.Dead.Contains(e)));
                            break;
                        }
                    }
                    break;
                case TargetTypes.FriendArea:
                    foreach (var factionName in gridEncounter.Factions.Keys.ToList())
                    {
                        var faction = gridEncounter.Factions[factionName];
                        if (faction.Contains(SourceEntity))
                        {
                            foreach (var target in faction)
                            {
                                TargetEntities.Add((GridEntity)target);
                            }

                        }
                    }
                    break;
            }

            return Task.CompletedTask;
        }

        public override void AppendToOperation(IOperationBuilder operationBuilder)
        {
            var builder = (MudLikeOperationBuilder)operationBuilder;

            if (TargetEntities.Count == 0)
                return;

            builder
                .StartContainer("action");

            foreach (var target in TargetEntities)
            {
                var primaryAffectedStat = Modifiers.FirstOrDefault();
                if (primaryAffectedStat == null)
                    continue;

                var elementInfo = Element.GetRange(primaryAffectedStat.Value);

                // dude's actionName hits target for # statname
                builder.AddText($"{SourceEntity.Name}'s ")
                    .AddText($"{Name}", Element.TextColor)
                    .AddText($" {elementInfo.Text}", elementInfo.TextColor)
                    .AddTextLine($" {target.Name}")
                  //  .AddText(" for ")
                  //  .AddTextLine($" {Math.Abs(primaryAffectedStat.Value)}", elementInfo.TextColor)
                    ;

            }



            //   .AddText($"{DamageDone}", TextColor.Red)
            //   .AddTextLine(" damage")
            builder.EndContainer("action");

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }



    }
}
