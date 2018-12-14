using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame
{
    public class BasicPlayer : IPlayer
    {
        public string Name { get; set; }

        public string NetworkId { get; set; }

        public IMap Map { get; set; }
    }
}
