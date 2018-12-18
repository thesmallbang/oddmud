using Microsoft.Extensions.Logging;
using OddMud.Core.Interfaces;
using OddMud.View.MudLike;
using System.Text;

namespace OddMud.ViewBuilders.MudLikeHtml
{
    public class MudLikeHtmlBuilder : IViewBuilder<string>
    {


        private readonly ILogger<MudLikeHtmlBuilder> _logger;

        public MudLikeHtmlBuilder(ILogger<MudLikeHtmlBuilder> logger)
        {
            _logger = logger;
            _logger.LogDebug("Injection : IViewBuilder");
        }

        public string Build(IViewItem viewItem)
        {
            string html = string.Empty;

            var tswitch = new TypeSwitch()
                .Case((TextItem item) => html = BuildTextItem(item))
                .Case((LineBreakItem item) => html = "<br />")
                .Case((ContainerStart item) => html = BuildContainerStart(item))
                .Case((ContainerEnd item) => html = $"</div><!-- {item.Id} -->")
                ;

            tswitch.Switch(viewItem);
            return html;
        }


        private string BuildTextItem(TextItem item)
        {
            if (string.IsNullOrEmpty(item.Text))
                return string.Empty;

            var output = $"<span class='textitem";

            if (item.Size != TextSize.Normal)
                output += " text-size-" + item.Size.ToString().ToLower();

            if (item.Color != TextColor.Normal)
            {
                output += " text-color-" + item.Color.ToString().ToLower();
            }

            // close our class list and item
            output += $"'>{item.Text}</span>";
            return output;
        }

        private string BuildContainerStart(ContainerStart item)
        {
            return $"<div id='{item.Id}' class='world-inline-container'>";
        }

      

    }
}
