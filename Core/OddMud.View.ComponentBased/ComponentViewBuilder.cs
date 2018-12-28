using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace OddMud.View.ComponentBased
{
    public class ComponentViewBuilder<T> : IViewCommand
    {

        public List<ViewComponent<T>> ViewComponents { get; set; } = new List<ViewComponent<T>>();


        public static ComponentViewBuilder<T> Start() => new ComponentViewBuilder<T>();

        public ComponentViewBuilder<T> AddComponent(T componentType,  object componentData)
        {
            var component = new ViewComponent<T>() { componentType = componentType, data = componentData };
            ViewComponents.Add(component);
            return this;
        }
                

     
    }



    public class ViewComponent<TEnum> : IViewItem
    {
        public TEnum componentType { get; set; }
        public object data { get; set; }

    }
}
