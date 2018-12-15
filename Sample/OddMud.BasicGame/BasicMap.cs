﻿using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OddMud.BasicGame
{
    public class BasicMap : IMap
    {
        public string Id { get; private set; } = "0";

        public string Name { get; private set; } = "Unconfigured";

        public string Description { get; private set; } = "Unconfigured";

        public IReadOnlyList<IPlayer> Players => _players;
        private List<IPlayer> _players = new List<IPlayer>();

        public BasicMap()
        {

        }

        public BasicMap(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

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


        public override string ToString()
        {
            return $"{Id}-{Name}";
        }
    }
}
