using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{

    public class ContainerEnd : IViewItem
    {
        public string Id { get; set; }

        public ContainerEnd(string id)
        {
            Id = id;
        }
    }
}
