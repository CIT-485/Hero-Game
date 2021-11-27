using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFade : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    public void Fade()
    {
        StartCoroutine(Timing());
    }

    IEnumerator Timing()
    {
        foreach (ParticleSystem ps in particles)
            ps.Stop(true);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
