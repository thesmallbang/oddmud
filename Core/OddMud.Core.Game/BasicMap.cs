using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.Core.Game
{
    public abstract class BasicMap : IMap
    {

        public int Id { get; set; }

        public string Name { get; set; } = "Unconfigured";

        public string Description { get; set; } = "Unconfigured";


        public IReadOnlyList<IPlayer> Players => _players;
        private List<IPlayer> _players = new List<IPlayer>();

        public IReadOnlyList<IItem> Items => _items;
        private List<IItem> _items = new List<IItem>();
        public IReadOnlyList<IEntity> Entities => _entities;
        private List<IEntity> _entities = new List<IEntity>();

        public event Func<IMap, IReadOnlyList<IItem>, Task> ItemsChanged;
        public event Func<IMap, IReadOnlyList<IPlayer>, Task> PlayersChanged;
        public event Func<IMap, IReadOnlyList<IEntity>, Task> EntitiesChanged;



        /// <summary>
        /// You should probably be using World.MovePlayer in most scenarios to make sure all the correct events are triggered
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual async Task AddPlayerAsync(IPlayer player)
        {
            player.Map = this;
            _players.Add(player);
            
            if (PlayersChanged != null)
                await PlayersChanged.Invoke(this, Players);

        }
        public virtual async Task RemovePlayerAsync(IPlayer player)
        {
            _players.Remove(player);
            if (PlayersChanged != null)
                await PlayersChanged.Invoke(this, Players);

        }

  
        public virtual async Task AddItemAsync(IItem item)
        {
            _items.Add(item);
            if (ItemsChanged != null)
                await ItemsChanged.Invoke(this, Items);


        }
        public virtual async Task RemoveItemAsync(IItem item)
        {
            _items.Remove(item);
            if (ItemsChanged != null)
                await ItemsChanged.Invoke(this, Items);

        }


        public virtual async Task AddEntityAsync(IEntity entity)
        {
            _entities.Add(entity);
            if (EntitiesChanged != null)
                await EntitiesChanged.Invoke(this, Entities);

        }

        public virtual async Task RemoveEntityAsync(IEntity entity)
        {
            _entities.Remove(entity);
            if (EntitiesChanged != null)
                await EntitiesChanged.Invoke(this, Entities);
        }



    }
}
