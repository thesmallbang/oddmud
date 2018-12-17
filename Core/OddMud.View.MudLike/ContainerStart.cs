using OddMud.Core.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace OddMud.View.MudLike
{

    public class ContainerStart : IViewItem
    {
        public string Id { get; }
        public ContainerStart(string id)
        {
            Id = id;
        }
    }
}
