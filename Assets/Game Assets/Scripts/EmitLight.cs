using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EmitLight : MonoBehaviour
{
    Light2D light2D;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (light2D.pointLightOuterRadius < 20)
            light2D.pointLightOuterRadius *= 1.1f;
    }
}
