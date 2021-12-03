using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public PlayerDataSO playerData;
    private static GameMaster instance;
    public Vector2 lastRespawnPos;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlayerStat playerStat = player.GetComponent<PlayerStat>();
        InventorySystem playerInv = player.GetComponent<InventorySystem>();
        playerData.str = playerStat.strength.BaseValue;
        playerData.vit = playerStat.vitality.BaseValue;
        playerData.agi = playerStat.agility.BaseValue;
        playerData.currenthealth = player.healthBar.currentHealth;
        playerData.corruption = player.corruption;
        playerData.pointsAvailable = playerInv.originalPointsAvailable;
        playerData.nextPointThreshold = playerInv.nextPointThreshold;
        playerData.scene = SceneManager.GetActiveScene().buildIndex;
        playerData.lastRespawnPos = lastRespawnPos;
        List<AbilitySlot> abList = player.GetComponent<InventorySystem>().abilities;
        for (int i = 0; i < abList.Count; i++)
            playerData.abilityUnlocked[i] = abList[i].unlocked;
    }
}
