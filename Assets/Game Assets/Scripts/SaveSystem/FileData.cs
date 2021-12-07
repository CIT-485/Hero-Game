using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FileData
{
    public float str = 10;
    public float vit = 10;
    public float agi = 10;
    public int currenthealth = 250;
    public int corruption;
    public int pointsAvailable;
    public int nextPointThreshold = 100;
    public int scene = 0;
    public float lastRespawnPosX = -16f;
    public float lastRespawnPosY = -1f;
    public List<bool> abilityUnlocked = new List<bool>();

    public void Copy(PlayerDataSO data)
    {
        str = data.str;
        vit = data.vit;
        agi = data.agi;
        currenthealth = data.currenthealth;
        corruption = data.corruption;
        pointsAvailable = data.pointsAvailable;
        nextPointThreshold = data.nextPointThreshold;
        scene = data.scene;
        lastRespawnPosX = data.lastRespawnPos.x;
        lastRespawnPosY = data.lastRespawnPos.y;
        abilityUnlocked = data.abilityUnlocked;
    }
    public void Copy(FileData data)
    {
        str = data.str;
        vit = data.vit;
        agi = data.agi;
        currenthealth = data.currenthealth;
        corruption = data.corruption;
        pointsAvailable = data.pointsAvailable;
        nextPointThreshold = data.nextPointThreshold;
        scene = data.scene;
        lastRespawnPosX = data.lastRespawnPosX;
        lastRespawnPosY = data.lastRespawnPosY;
        abilityUnlocked = data.abilityUnlocked;
    }
}