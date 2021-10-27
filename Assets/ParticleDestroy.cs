using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Particles")
        {
            var parts = collision.GetComponent<ParticleTest>();
            parts.Fade();
        }
    }

}
