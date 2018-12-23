using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.ViewConverters.MudLikeHtml
{
    // https://stackoverflow.com/a/7301514/1832520
    public class TypeSwitch
    {
        Dictionary<Type, Action<object>> matches = new Dictionary<Type, Action<object>>();
        public TypeSwitch Case<T>(Action<T> action) { matches.Add(typeof(T), (x) => action((T)x)); return this; }
        public void Switch(object x) { matches[x.GetType()](x); }
    }
}
