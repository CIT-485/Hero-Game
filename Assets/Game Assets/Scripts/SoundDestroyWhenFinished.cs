using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestroyWhenFinished : MonoBehaviour
{
    AudioSource aud;
    private void Start()
    {
        aud = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!aud.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
