
using OddMud.Core.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace OddMud.BasicGame.Commands
{
    public class CommandModel 
    {

        private List<string> _parts;

        public string RawCommand { get; set; }

        public IReadOnlyList<string> Parts
        {
            get
            {
                if (_parts == null)
                {
                    _parts = RawCommand.Split(' ').ToList();
                }
                return _parts;
            }
        }

        public string FirstPart => Parts[0];

        public string StringFrom(int index)
        {
            return string.Join(' ', Parts.Skip(index));
        }

    }
}
