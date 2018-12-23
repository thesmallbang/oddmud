using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IProperty
    {
        string Name { get; set; }

    }

    public interface IProperty<T> : IProperty
    {
        T Value { get; }
    }
}
