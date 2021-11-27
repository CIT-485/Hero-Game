using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCollection : MonoBehaviour
{
    public float radius = 1;
    public float maxRadius = 10;
    ParticleFade pf;
    public SpriteRenderer sr;
    private void Start()
    {
        pf = GetComponent<ParticleFade>();
    }
    // Update is called once per frame
    void Update()
    {
        sr.gameObject.transform.localScale = new Vector3(radius, radius, radius);
        GameObject.Find("AbsorbCircle").transform.localScale = new Vector3(radius, radius, radius);
        radius *= 1.02f;
        if (radius >= maxRadius)
        {
            GameObject.Find("AbsorbCircle").transform.localScale = new Vector3(1, 1, 1);
            Destroy(gameObject);
        }
    }
}
