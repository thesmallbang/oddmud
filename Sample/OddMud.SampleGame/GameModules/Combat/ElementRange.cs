using OddMud.Core.Interfaces;



namespace OddMud.SampleGame.GameModules.Combat
{

    public class ElementRange : IElementRange
    {
        public int? Min { get; set; }
        public int? Max { get; set; }

        public string Text { get; set; }


    }
}
