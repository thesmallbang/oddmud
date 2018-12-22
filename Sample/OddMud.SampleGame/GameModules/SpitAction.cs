using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;
using OddMud.View.MudLike;

namespace OddMud.SampleGame.GameModules
{
    public class SpitAction : ICombatAction<GridEntity>
    {

        public GridEntity SourceEntity { get; set; }

        public GridEntity TargetEntity { get; set; }

        private int _damageDone;
        private Random _randomizer = new Random();



        public async Task Execute()
        {

            if (TargetEntity == null)
            {
                throw new Exception("SpitAction requires a target");
            }

            var dmg = _randomizer.Next(1, 3);

            // apply the dmg ..stats missing
            var hpstat = TargetEntity.Stats.FirstOrDefault(s => s.Name == "health");
            if (hpstat == null)
                return;

            await hpstat.ApplyToCurrentAsync(-dmg);
            _damageDone = dmg;

        }

        public Task SetDefaultTargetAsync(IEnumerable<GridEntity> entities)
        {
            var otherEntities = entities.Where(e => e.IsAlive).Except(new List<GridEntity>() { SourceEntity }).ToList();

            if (otherEntities.Count == 0)
                return Task.CompletedTask;

            // implement some sort of hate system using combatant.Stats to alter results here..
            var isSourcePlayer = SourceEntity.GetType().GetInterfaces().Contains(typeof(IPlayer));
            if (isSourcePlayer)
            {
                // find a monster
                var monsters = otherEntities.Where(e => !e.GetType().GetInterfaces().Contains(typeof(IPlayer))).ToList();
                TargetEntity = monsters.FirstOrDefault();
            }
            else
            {
                // find a player
                var players = otherEntities.Where(e => e.GetType().GetInterfaces().Contains(typeof(IPlayer))).ToList();
                TargetEntity = players.FirstOrDefault();
            }


            return Task.CompletedTask;
        }

        public IViewCommand<IViewItem> ToView()
        {
            return MudLikeCommandBuilder.Start()
                .StartContainer("action")
                .AddText($"{SourceEntity.Name} ")
                .AddText("spits", TextColor.Aqua)
                .AddText("on")
                .AddText($"{TargetEntity.Name} for ")
                .AddText($"{_damageDone}", TextColor.Red)
                .AddTextLine(" damage")
                .EndContainer("action")
                .Build(ViewCommandType.Append);

            //return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }

        public string ToMessage()
        {
            return $"{SourceEntity?.Name} spit on {TargetEntity?.Name} for {_damageDone}";
        }


    }
}
