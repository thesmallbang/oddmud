using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{

    
    public class LinkItem : TextItem
    {
        public string Command { get; set; }

        public LinkItem(string text, string command)
        {
            Text = text;
            Command = command;
        }
       
    }
}
