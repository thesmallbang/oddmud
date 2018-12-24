using OddMud.Core.Interfaces;

namespace OddMud.SampleGame.GameModules.Combat
{
    public interface IActionModifier : IProperty<int>
    {
        ActionModifierType ModifierType { get; set; }

        int Max { get; set; }
        int Min { get; set; }

        // does the modifier apply to the target or the caster?
        TargetTypes TargetType { get; set; }


    }
}