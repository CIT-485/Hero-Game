using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHitAudio : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox")
        {
            FindObjectOfType<Audio_Player>().PlaySound("AWP_Impact_Body_08");
        }
    }
}
