using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ChangeBackground : MonoBehaviour
{
    private Transform player;
    private Color prevColor;
    private bool done = true;
    public List<bool> queue = new List<bool>();

    public GameObject background1;
    public bool background1Active = true;
    public GameObject background2;
    public bool background2Active = false;
    public float xThreshold = 0;
    public float fadeTime = 1;
    
    [Space(10)]
    
    public Light2D globalLight;
    [Range(0, 1f)]
    public float r = 1;
    [Range(0, 1f)]
    public float g = 1;
    [Range(0, 1f)]
    public float b = 1;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        prevColor = globalLight.color;
    }
    private void Update()
    {
        if (player.position.x > xThreshold)
        {
            if (queue.Count > 0)
            {
                if (!queue[queue.Count - 1])
                {
                    queue.Add(true);
                }
            }
            else
            {
                queue.Add(true);
            }
        }
        else if (player.position.x < xThreshold)
        {
            if (queue.Count > 0)
            {
                if (queue[queue.Count - 1])
                {
                    queue.Add(false);
                }
            }
            else
            {
                queue.Add(false);
            }
        }
        if (queue.Count > 0)
        {
            if (done == true)
            {
                if (queue[0] && player.position.x > xThreshold)
                {
                    queue.RemoveAt(0);
                    FadeOut(background1, fadeTime);
                    FadeIn(background2, fadeTime);
                    Color newColor = new Color(r, g, b);
                    StartCoroutine(ChangeLightsColor(newColor));
                }
                else if (!queue[0] && player.position.x < xThreshold)
                {
                    queue.RemoveAt(0);
                    FadeIn(background1, fadeTime);
                    FadeOut(background2, fadeTime);
                    StartCoroutine(ChangeLightsColor(prevColor));
                }
            }
        }
    }
    public void FadeOut(GameObject bg, float fadeTime)
    {
        done = false;
        if (bg.GetComponent<SpriteRenderer>())
        {
            StartCoroutine(FadeOutAction(bg.GetComponent<SpriteRenderer>(), fadeTime));
        }
        for (int i = 0; i < bg.transform.childCount; i++)
        {
            if (bg.transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                StartCoroutine(FadeOutAction(bg.transform.GetChild(i).GetComponent<SpriteRenderer>(), fadeTime));
                for (int j = 0; j < bg.transform.GetChild(i).childCount; j++)
                {
                    if (bg.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                    {
                        StartCoroutine(FadeOutAction(bg.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>(), fadeTime));
                    }
                }
            }
        }
        done = true;
    }
    public void FadeIn(GameObject bg, float fadeTime)
    {
        done = false;
        if (bg.GetComponent<SpriteRenderer>())
        {
            StartCoroutine(FadeInAction(bg.GetComponent<SpriteRenderer>(), fadeTime));
        }
        for (int i = 0; i < bg.transform.childCount; i++)
        {
            if (bg.transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                StartCoroutine(FadeInAction(bg.transform.GetChild(i).GetComponent<SpriteRenderer>(), fadeTime));
                for (int j = 0; j < bg.transform.GetChild(i).childCount; j++)
                {
                    if (bg.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                    {
                        StartCoroutine(FadeInAction(bg.transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>(), fadeTime));
                    }
                }
            }
        }
        done = true;
    }
    IEnumerator FadeInAction(SpriteRenderer sprite, float fadeTime)
    {
        Color color = sprite.color;

        while (color.a < 1)
        {
            color.a += Time.deltaTime / fadeTime;
            if (color.a >= 1)
                color.a = 1;
            sprite.color = color;
            yield return null;
        }
        sprite.color = color;
    }
    IEnumerator FadeOutAction(SpriteRenderer sprite, float fadeTime)
    {
        Color color = sprite.color;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / fadeTime;
            if (color.a <= 0)
                color.a = 0;
            sprite.color = color;
            yield return null;
        }
        sprite.color = color;
    }

    IEnumerator ChangeLightsColor(Color targetColor)
    {
        Color tmpColor = globalLight.color;
            float rDiff = targetColor.r - tmpColor.r;
            float gDiff = targetColor.g - tmpColor.g;
            float bDiff = targetColor.b - tmpColor.b;
        while (tmpColor.r != targetColor.r && tmpColor.g != targetColor.g && tmpColor.b != targetColor.b)
        {
            tmpColor.r += rDiff * Time.deltaTime / fadeTime;
            tmpColor.g += gDiff * Time.deltaTime / fadeTime;
            tmpColor.b += bDiff * Time.deltaTime / fadeTime;
            if (rDiff > 0 && tmpColor.r > targetColor.r)
                tmpColor.r = targetColor.r;
            else if (rDiff < 0 && tmpColor.r < targetColor.r)
                tmpColor.r = targetColor.r;
            if (gDiff > 0 && tmpColor.g > targetColor.g)
                tmpColor.g = targetColor.g;
            else if (gDiff < 0 && tmpColor.g < targetColor.g)
                tmpColor.g = targetColor.g;
            if (bDiff > 0 && tmpColor.b > targetColor.b)
                tmpColor.b = targetColor.b;
            else if (bDiff < 0 && tmpColor.b < targetColor.b)
                tmpColor.b = targetColor.b;
            globalLight.color = tmpColor;
            yield return null;
        }
    }
}
