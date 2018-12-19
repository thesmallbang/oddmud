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

        public IReadOnlyList<IItem> Items => _items;
        private List<IItem> _items = new List<IItem>();
        public IReadOnlyList<IEntity> Npcs => _npcs;
        private List<IEntity> _npcs { get; set; }


        private List<IPlayer> _players = new List<IPlayer>();

        /// <summary>
        /// You should probably be using World.MovePlayer in most scenarios to make sure all the correct events are triggered
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public virtual Task AddPlayerAsync(IPlayer player)
        {
            player.Map = this;
            _players.Add(player);
            return Task.CompletedTask;
        }
        public virtual Task RemovePlayerAsync(IPlayer player)
        {
            _players.Remove(player);
            return Task.CompletedTask;
        }

  
        public virtual Task AddItemAsync(IItem item)
        {
            _items.Add(item);
            return Task.CompletedTask;
        }
        public virtual Task RemoveItemAsync(IItem item)
        {
            _items.Remove(item);
            return Task.CompletedTask;
        }


        public virtual Task AddNpcAsync(IEntity npc)
        {
            _npcs.Add(npc);
            return Task.CompletedTask;
        }
        public virtual Task RemoveNpcAsync(IEntity npc)
        {
            _npcs.Remove(npc);
            return Task.CompletedTask;
        }



    }
}
