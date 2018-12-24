using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules
{
    public interface IActionModifier : IProperty<int>
    {
        ActionModifierType ModifierType { get; set; }

        int Max { get; set; }
        int Min { get; set; }

    }
}