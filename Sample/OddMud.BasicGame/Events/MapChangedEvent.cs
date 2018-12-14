using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Events
{
    class MapChangedEvent : EventArgs, IMapChangeEvent
    {
        public IPlayer Player { get; }

        public IMap OldMap { get; }

        public IMap NewMap { get; }

        public MapChangedEvent(IPlayer player, IMap oldMap, IMap newMap)
        {
            Player = player;
            OldMap = oldMap;
            NewMap = newMap;
        }
    }
}
