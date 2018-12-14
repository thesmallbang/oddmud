using OddMud.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OddMud.BasicGame.Commands
{
    public class SetViewCommand : IViewCommand
    {
        public ViewCommandType CommandType => ViewCommandType.Set;

        public ViewLocation ViewLocation => new ViewLocation() { X = 0, Y = 0, Z = 0 };

        public string Markdown { get; private set; }

        public SetViewCommand(string markdown)
        {

        }
    }
}
