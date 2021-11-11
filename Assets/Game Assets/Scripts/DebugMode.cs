using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour
{
    public bool debugMode = false;
    GameObject hero;
    GameObject[] points;
    public int cnt;

    private void Start()
    {
        points = GameObject.FindGameObjectsWithTag("Respawn");
        hero = GameObject.FindGameObjectWithTag("Player");
        cnt = points.Length - 1;
        Debug.Log(cnt);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugMode = !debugMode;
        }
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

        if (Input.GetKeyDown("i"))
        {
            if (cnt < 0)
            {
                cnt = points.Length - 1;
            }
            hero.transform.position = new Vector3((points[cnt].transform.position.x), (points[cnt].transform.position.y));
            cnt = cnt - 1;
        }
    }
}
