namespace Kryz.CharacterStats
{

    // Used to define two types of stat modifiers flat (whole numbers) and percentages
    public enum StatModType
    {
        // order of 0, applied first
        Flat = 100,
        // order of 1, applied second
        PercentAdd = 200,
        PercentMult = 300,
    }
    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order; // determines the order in which stat modifiers are added
        public readonly object Source;
        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null)
        {

        }

        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null)
        {

        }

        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source)
        {

        }
    }

}