using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMapPosition : MonoBehaviour
{
    public Vector2 startingPosition;
    // Start is called before the first frame update
    void Awake()
    {
        GameMaster gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.playerData.lastRespawnPos = startingPosition;
        gm.lastRespawnPos = startingPosition;
    }
}
