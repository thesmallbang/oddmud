using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.ViewComponents
{
    public class WorldComponentData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Exits { get; set; }

    }
}
