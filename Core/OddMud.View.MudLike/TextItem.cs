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

        public TextItem()
        {

        }
        public TextItem(string text)
        {
            Text = text;
        }
        public TextItem(string text, TextColor color)
        {
            Text = text;
            Color = color;
        }

        public TextItem(string text, TextSize size)
        {
            Text = text;
            Size = size;
        }

        public TextItem(string text, TextColor color, TextSize size)
        {
            Text = text;
            Color = color;
            Size = size;
        }
    }
}
