using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHitAudio : MonoBehaviour
{
    //public AudioSource hitNoise;
    public Sound[] sounds;
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
