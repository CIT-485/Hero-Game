using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Light2DFade : MonoBehaviour
{
    private bool fade;
    private float time;
    public bool destroy;
    public bool fadeIn;
    public float targetIntensity = 0;
    public List<Light2D> light2Ds = new List<Light2D>();
    private List<float> originalIntensity = new List<float>();
    private void Awake()
    {
        foreach (Light2D light in light2Ds)
        {
            originalIntensity.Add(light.intensity);
            if (fadeIn)
                light.intensity = targetIntensity;
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < light2Ds.Count; i++)
        {
            if (fadeIn)
                light2Ds[i].intensity = targetIntensity;
            else
                light2Ds[i].intensity = originalIntensity[i];
        }
    }
    private void Update()
    {
        if (fadeIn && !fade)
            for (int i = 0; i < light2Ds.Count; i++)
            {
                light2Ds[i].intensity += (originalIntensity[i] - targetIntensity) * Time.deltaTime / time;
                if (light2Ds[i].intensity > originalIntensity[i])
                    light2Ds[i].intensity = originalIntensity[i];
            }
        if (fade)
            for (int i = 0; i < light2Ds.Count; i++)
            {
                light2Ds[i].intensity -= (originalIntensity[i] - targetIntensity) * Time.deltaTime / time;
                if (light2Ds[i].intensity < targetIntensity)
                    light2Ds[i].intensity = targetIntensity;
            }
    }
    public void Fade(float time)
    {
        if (fadeIn)
            StartCoroutine(Timing2(time));
        else
            StartCoroutine(Timing(time));
    }

    IEnumerator Timing2(float time)
    {
        yield return new WaitForSeconds(0.03f);
        this.time = time;
        yield return new WaitUntil(() => light2Ds[0].intensity >= originalIntensity[0]);
        fade = true;
        yield return new WaitForSeconds(time);
        fade = false;
        if (destroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
    IEnumerator Timing(float time)
    {
        fade = true;
        this.time = time;
        yield return new WaitForSeconds(time);
        fade = false;
        if (destroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
