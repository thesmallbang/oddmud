using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.Core.Interfaces
{
    public interface IViewCommand
    {
        ViewCommandType CommandType { get; }
        ViewLocation ViewLocation { get; }
        string Markdown { get; }

    }

    public enum ViewCommandType
    {
        Set,
        Append
    }
    public class ViewLocation
    {
        public int X = 0;
        public int Y = 0;
        // layer / draw order
        public int Z = 0;
    }
}
