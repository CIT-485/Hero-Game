using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHitAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerHitbox" && !GetComponent<Enemy>().IsDead)
        {
            //FindObjectOfType<Audio_Player>().PlaySound("Hit noise");
            audioSource.Play();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
