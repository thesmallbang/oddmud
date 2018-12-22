using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeOperationBuilder : IOperationBuilder
    {
        private ViewOperationType _commandType;
        private string _relatedId;
        private List<IViewItem> viewItems = new List<IViewItem>();

        public MudLikeOperationBuilder(ViewOperationType commandType = ViewOperationType.Set, string relatedId = "")
        {
            _commandType = commandType;
            _relatedId = relatedId;
        }

        public static MudLikeOperationBuilder Start(ViewOperationType commandType, string relatedId = "")
        {
            return new MudLikeOperationBuilder(commandType, relatedId);
        }
        public static MudLikeOperationBuilder Start(string relatedId = "")
        {
            return new MudLikeOperationBuilder(ViewOperationType.Set, relatedId);
        }


        public IViewOperation<IViewItem> Build()
        {
            return new MudLikeOperation(_commandType, viewItems, _relatedId);
        }

        public MudLikeOperationBuilder StartContainer(string id)
        {

            viewItems.Add(new ContainerStart(id));
            return this;
        }
        public MudLikeOperationBuilder EndContainer(string id)
        {

            viewItems.Add(new ContainerEnd(id));
            return this;
        }


        public MudLikeOperationBuilder AddText(string message, TextColor color = TextColor.Normal, TextSize size = TextSize.Normal)
        {
            viewItems.Add(new TextItem(message, color, size));
            return this;
        }

        public MudLikeOperationBuilder AddTextLine(string message, TextColor color = TextColor.Normal, TextSize size = TextSize.Normal)
        {
            AddText(message, color, size);
            AddLineBreak();
            return this;
        }

        public MudLikeOperationBuilder AddLineBreak()
        {
            viewItems.Add(new LineBreakItem());
            return this;
        }

    }
}
