
using OddMud.Core.Plugins;
using System.Collections.Generic;
using System.Linq;

namespace OddMud.BasicGame.Commands
{
    public class CommandModel 
    {

        private List<string> _parts;

        public string RawCommand { get; set; }

        public List<string> Parts
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

        public string StringFrom(int index)
        {
            var parts = Parts.GetRange(index, Parts.Count - index);
            return string.Join(' ', parts);
        }

    }
}
