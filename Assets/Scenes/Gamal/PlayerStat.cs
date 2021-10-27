using UnityEngine;
using Kryz.CharacterStats;
public class PlayerStat : MonoBehaviour
{
    public CharacterStat playerAttack;
    public CharacterStat playerDefense;
    public CharacterStat playerHealth;

    private void Start()
    {
        playerAttack.BaseValue = 16;
        playerDefense.BaseValue = 22;
        playerHealth.BaseValue = 22;
    }

}
