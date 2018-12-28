using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace OddMud.View.ComponentBased
{
    public class ComponentViewBuilder<T>
    {

        public List<ViewComponent<T>> ViewComponents { get; set; }

        public static ComponentViewBuilder<T> Start() => new ComponentViewBuilder<T>();

        public void AddComponent(T componentType,  string name, object componentData)
        {
            ViewComponents.Add(new ViewComponent<T>() { componentType = componentType, name = name, data = componentData });        
        }

        

    }

    public class ViewComponent<TEnum> : IViewItem
    {
        public TEnum componentType { get; set; }
        public string name { get; set; }
        public object data { get; set; }

    }
}
