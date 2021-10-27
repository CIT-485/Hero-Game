using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats;
public class EnemyStat : MonoBehaviour
{
    public CharacterStat Attack;
    public CharacterStat Defense; // damage reduction from enemies
    public CharacterStat Health; // amount of health points

    /*
    private void Start()
    {
        Attack.BaseValue = 4;
    }
    
    */
}
