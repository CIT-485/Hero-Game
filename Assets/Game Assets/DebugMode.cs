using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public bool debugMode = false;
    void Update()
    {
        if (debugMode || !debugMode)
        {
            GameObject[] playerHitboxes = GameObject.FindGameObjectsWithTag("PlayerHitbox");
            GameObject[] enemyHitboxes = GameObject.FindGameObjectsWithTag("EnemyHitbox");
            GameObject[] draws = GameObject.FindGameObjectsWithTag("Draw");

            foreach (GameObject g in playerHitboxes)
            {
                g.GetComponent<SpriteRenderer>().enabled = debugMode;
            }
            foreach (GameObject g in enemyHitboxes)
            {
                g.GetComponent<SpriteRenderer>().enabled = debugMode;
            }
            foreach (GameObject g in draws)
            {
                g.GetComponent<SpriteRenderer>().enabled = debugMode;
            }
        }
    }
}
