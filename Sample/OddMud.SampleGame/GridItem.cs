using OddMud.Core.Game;
using OddMud.SampleGame.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.SampleGame
{
    public class GridItem : BasicItem
    {

        public readonly List<ItemTypes> ItemTypes = new List<ItemTypes>() { Misc.ItemTypes.Normal };


        public GridItem(int id,  string name, string description, List<ItemTypes> itemTypes)
        {
            Id = id;
            Name = name;
            Description = description;
            ItemTypes = itemTypes;
        }
    }
}
