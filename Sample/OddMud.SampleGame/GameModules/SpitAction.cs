using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public class SpitAction : ICombatAction<GridEntity>
    {
        public GridEntity SourceEntity { get; set; }

        public GridEntity TargetEntity { get; set; }

        public Task Execute()
        {

            if (TargetEntity == null)
            {
                throw new Exception("SpitAction requires a target");
            }



            return Task.CompletedTask;
        }
    }
}
