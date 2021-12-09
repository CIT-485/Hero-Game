using UnityEngine;
using Kryz.CharacterStats;
public class PlayerStat : MonoBehaviour
{
    public CharacterStat strength;
    public CharacterStat vitality;
    public CharacterStat agility;
    private void Awake()
    {
        strength.BaseValue = 10;
        vitality.BaseValue = 10;
        agility.BaseValue = 10;
    }
}