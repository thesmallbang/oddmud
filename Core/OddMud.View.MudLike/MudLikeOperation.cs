using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeOperation : IViewOperation<IViewItem>
    {
        public ViewOperationType OperationType { get; }

        public IEnumerable<IViewItem> Data { get; private set; }

        public string RelatedId { get; set; }

        public MudLikeOperation(ViewOperationType operationType, IEnumerable<IViewItem> viewItems, string relatedId = "")
        {
            OperationType = operationType;
            Data = viewItems;
            RelatedId = relatedId;
        }
    }



}
