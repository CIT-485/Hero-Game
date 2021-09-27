using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlashingLight2D : MonoBehaviour
{
    public float timeSinceFlash = 0;
    public float flashIntervalTimer = 0.1f;
    public bool flashFade = false;
    public Light2D flash;
    float originalIntensity;
    private void Start()
    {
        flash = GetComponent<Light2D>();
        originalIntensity = flash.intensity;
    }
    private void OnDisable()
    {
        timeSinceFlash = 0;
        flash.intensity = originalIntensity;
    }
    void Update()
    {
        timeSinceFlash += Time.deltaTime;

        if (timeSinceFlash > flashIntervalTimer)
        {
            flash.enabled = !flash.enabled;
            if (flash.enabled)
                flash.intensity = originalIntensity;
            timeSinceFlash = 0;
        }

        if (flash.enabled)
        {
            if (flashFade)
                if (flash.intensity > 0)
                    flash.intensity -= originalIntensity * Time.deltaTime / flashIntervalTimer;
        }
    }
}
