using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{

    // didnt need to restrict ourselfs with the size and color here but ...mudlike...
    public class TextItem : IViewItem
    {
        public string Text { get; set; }
        
        public TextSize Size { get; set; } = TextSize.Normal;
        public TextColor Color { get; set; }
    }
}
