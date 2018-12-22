using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OddMud.View.MudLike
{
    public class MudLikeViewBuilder : IViewBuilder
    {


        private List<IViewOperation<IViewItem>> _operations = new List<IViewOperation<IViewItem>>();


        public static MudLikeViewBuilder Start()
        {
            return new MudLikeViewBuilder();
        }


        public MudLikeViewBuilder AddOperation(IViewOperation<IViewItem> operation)
        {
            _operations.Add(operation);
            return this;
        }



        public IViewCommand<IViewItem> Build()
        {

            return new MudLikeViewCommand(_operations.Select(o => (MudLikeOperation)o).ToList());

        }

    }
}
