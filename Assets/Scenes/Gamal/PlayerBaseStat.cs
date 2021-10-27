using UnityEngine;
using Kryz.CharacterStats;
public class PlayerBaseStat : MonoBehaviour
{
    private CharacterStat playerAttack;
    private CharacterStat playerDefense;
    private CharacterStat playerHealth;
    public float currentAtk;
    public float currentDef;
    public float currentHP;

    private void Start()
    {
        currentAtk = playerAttack.BaseValue = 16;
        currentDef = playerDefense.BaseValue = 10;
        currentHP = playerHealth.BaseValue = 22;
    }

}
