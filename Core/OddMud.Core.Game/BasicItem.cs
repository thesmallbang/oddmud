using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public class BasicItem : IItem
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual int Id { get; set; }

        public IReadOnlyList<IStat> Stats => _stats;
        private List<BasicStat> _stats = new List<BasicStat>();


        public event Func<IItem, IEntity, Task> PickedUp;
        public event Func<IItem, IEntity, Task> Dropped;


        public Task MarkAsPickedUpAsync(IEntity entityWhoPickedUp)
        {
            if (PickedUp != null)
                return PickedUp.Invoke(this, entityWhoPickedUp);

            return Task.CompletedTask;
        }

        public Task MarkAsDroppedAsync(IEntity entityWhoDropped)
        {
            if (Dropped != null)
                return Dropped.Invoke(this, entityWhoDropped);

            return Task.CompletedTask;
        }


        public BasicItem(IEnumerable<BasicStat> stats)
        {
            _stats.AddRange(stats);
        }

    }
}
