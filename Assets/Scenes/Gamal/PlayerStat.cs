using UnityEngine;
using Kryz.CharacterStats;
public class PlayerStat : MonoBehaviour
{
    public CharacterStat attack;
    public CharacterStat defense;
    public CharacterStat health;

    private void Start()
    {
        attack.BaseValue = 16;
        defense.BaseValue = 22;
        health.BaseValue = 22;
    }

}
