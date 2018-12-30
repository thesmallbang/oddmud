
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.SampleGame.ViewComponents
{
    public class EncounterData
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public List<PlayerData> Entities { get; set; } = new List<PlayerData>();

    }
}
