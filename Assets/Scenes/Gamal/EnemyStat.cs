using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;
public class EnemyStat : MonoBehaviour
{
    public CharacterStat enemyAttack;
    public CharacterStat enemyDefense; // damage reduction from enemies
    public CharacterStat enemyHealth; // amount of health points

    
    private void Start()
    {
        enemyAttack.BaseValue = 10;
        enemyDefense.BaseValue = 8;
        enemyHealth.BaseValue = 15;
    }
    
}
