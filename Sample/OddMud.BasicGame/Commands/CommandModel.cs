
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

        public string FirstPart => Parts.Count > 0 ? Parts[0] : "";
        public string SecondPart => Parts.Count > 1 ? Parts[1] : "";
        public string ThirdPart => Parts.Count > 2 ? Parts[2] : "";
        public string ForthPart => Parts.Count > 3 ? Parts[3] : "";



        public string StringFrom(int index)
        {
            return string.Join(' ', Parts.Skip(index));
        }

    }
}
