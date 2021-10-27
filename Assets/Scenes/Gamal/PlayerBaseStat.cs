using UnityEngine;
using Kryz.CharacterStats;
public class PlayerBaseStat : MonoBehaviour
{
    public CharacterStat Attack; 
    public CharacterStat Defense; // damage reduction from enemies
    public CharacterStat Health; // amount of health points

    
    private void Start()
    {
        Attack.BaseValue = 10;
        Defense.BaseValue = 10;
        Health.BaseValue = 10;
    }
    

}

