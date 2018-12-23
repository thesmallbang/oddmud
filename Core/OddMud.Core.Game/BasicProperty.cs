using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Game
{
    public class BasicProperty<T> : IProperty<T>
    {
        public T Value { get; set; }

        public string Name { get; set; }

        public BasicProperty(string name)
        {
            Name = name;
        }

        public BasicProperty(string name, T value)
            : this(name)
        {
            Value = value;
        }
    }
}
