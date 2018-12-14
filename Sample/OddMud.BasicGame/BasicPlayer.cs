using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame
{
    public class BasicPlayer : IPlayer
    {
        public string Name { get; set; }

        public string TransportId { get; set; }

        public IMap Map { get; set; }
    }
}
