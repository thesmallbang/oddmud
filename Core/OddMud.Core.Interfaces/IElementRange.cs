namespace OddMud.Core.Interfaces
{
    public interface IElementRange
    {
        int? Max { get; set; }
        int? Min { get; set; }
        string Text { get; set; }
    }
}