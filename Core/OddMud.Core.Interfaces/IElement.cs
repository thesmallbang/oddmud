using System.Collections.Generic;

namespace OddMud.Core.Interfaces
{
    public interface IElement
    {
        int Id { get; set; }
        string Name { get; set; }
        List<IElementRange> Ranges { get; set; }

    }
}