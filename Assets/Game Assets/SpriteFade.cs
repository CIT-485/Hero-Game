using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFade : MonoBehaviour
{
    public bool visible = true;
    public bool fadeOut = false;
    public bool fadeIn = false;
    void Update()
    {
        if (fadeOut)
        {
            FadeOut(1);
        }
        if (fadeIn)
        {
            FadeIn(1);
        }
    }

    public void ChangeVisibility(float fadeTime)
    {
        if (visible)
        {
            FadeOut(fadeTime);
        }
        else
        {
            FadeIn(fadeTime);
        }
    }

    public void FadeOut(float fadeTime)
    {
        visible = false;
        fadeOut = false;
        if (GetComponent<SpriteRenderer>())
        {
            StartCoroutine(FadeOutAction(GetComponent<SpriteRenderer>(), fadeTime));
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                StartCoroutine(FadeOutAction(transform.GetChild(i).GetComponent<SpriteRenderer>(), fadeTime));
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    if (transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                    {
                        StartCoroutine(FadeOutAction(transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>(), fadeTime));
                    }
                }
            }
        }
    }
    public void FadeIn(float fadeTime)
    {
        visible = true;
        fadeIn = false;
        if (GetComponent<SpriteRenderer>())
        {
            StartCoroutine(FadeInAction(GetComponent<SpriteRenderer>(), fadeTime));
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>())
            {
                StartCoroutine(FadeInAction(transform.GetChild(i).GetComponent<SpriteRenderer>(), fadeTime));
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    if (transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>())
                    {
                        StartCoroutine(FadeInAction(transform.GetChild(i).GetChild(j).GetComponent<SpriteRenderer>(), fadeTime));
                    }
                }
            }
        }
    }
    IEnumerator FadeInAction(SpriteRenderer sprite, float fadeTime)
    {
        Color color = sprite.color;

        while (color.a < 1)
        {
            color.a += Time.deltaTime / fadeTime;
            sprite.color = color;
            if (color.a >= 1)
                color.a = 1;
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
            sprite.color = color;
            if (color.a <= 0)
                color.a = 0;
            yield return null;
        }
        sprite.color = color;
    }
}
