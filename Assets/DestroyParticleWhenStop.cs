using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleWhenStop : MonoBehaviour
{
    public ParticleSystem ps;
    
    // Update is called once per frame
    void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
