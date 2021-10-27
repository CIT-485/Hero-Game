using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kryz.CharacterStats
{
    [Serializable]
    public class CharacterStat
    {
        // the base state value
        public float BaseValue;

        //
        // retrieves the caluclated final stat value
        // Only Caluclates values if changes have been made 
        public virtual float Value
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        // Checks to see if we need to re-calculate the stat value or not 
        protected bool isDirty = true;

        // Holds most recent stat calculation
        protected float _value;

        protected float lastBaseValue = float.MinValue;

        // list of stat modifiers that holds all of the stat modifiers
        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // users can see this but not change the stat modifiers

        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }
        // Initialize constructor
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        // Add modifier method
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            // order which modifiers are added
            statModifiers.Sort(CompareModifierOrder);
        }

        // comparison function to check the order of modifiers 
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            // first object comes before the second object in the list
            if (a.Order < b.Order)
            {
                return -1;
            }
            else if (a.Order > b.Order)// the first object should come after the second object
            {
                return 1;
            }
            else
            {
                return 0; // the two objects have an equal priority in the list
            }
        }

        // Remove modifier method
        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        // remove items from a list
        // loop through all stat modifiers and remove them from the specified source
        // traverse the list from reverse
        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }

            return didRemove;
        }
        // Calculates the final value of the stat
        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += statModifiers[i].Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    // Modifier: + 10%
                    // Value in code = 0.1
                    finalValue *= 1 + mod.Value;
                    // 1 + 0.1 (Value) = 1.1 = 110%
                    // Base Value = 10
                    // Increased by 10%
                    // 10 * 1.1 = 11
                }

            }

            // round value to 4 significant digits to avoid calculation erros
            return (float)Math.Round(finalValue, 4);
        }
    }

}