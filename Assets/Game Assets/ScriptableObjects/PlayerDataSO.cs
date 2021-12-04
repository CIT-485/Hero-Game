using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDataSO : ScriptableObject
{
    public PlayerDataSO baseData;
    public float str = 10;
    public float vit = 10;
    public float agi = 10;
    public int currenthealth = 250;
    public int corruption;
    public int pointsAvailable;
    public int nextPointThreshold = 100;
    public int scene = 0;
    public Vector2 lastRespawnPos = new Vector2(-16, -1);
    public List<bool> abilityUnlocked = new List<bool>();

    public void Reset()
    {
        Copy(baseData);
    }

    public void Copy(PlayerDataSO other)
    {
        str = other.str;
        vit = other.vit;
        agi = other.agi;
        currenthealth = other.currenthealth;
        corruption = other.corruption;
        pointsAvailable = other.pointsAvailable;
        nextPointThreshold = other.nextPointThreshold;
        scene = other.scene;
        lastRespawnPos = other.lastRespawnPos;
        abilityUnlocked = other.abilityUnlocked;
    }
}
