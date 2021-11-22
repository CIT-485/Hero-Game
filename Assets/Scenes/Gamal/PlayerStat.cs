using UnityEngine;
using Kryz.CharacterStats;
public class PlayerStat : MonoBehaviour
{
    public CharacterStat strength;
    public CharacterStat vitality;
    public CharacterStat dexterity;
    public CharacterStat physicalDef;
    public CharacterStat magicDef;
    public CharacterStat FireDef;
    public CharacterStat lightningDef;
    private void Start()
    {
        strength.BaseValue = 16;
        vitality.BaseValue = 22;
        dexterity.BaseValue = 22;
    }

}
