using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : MonoBehaviour
{
    float x = 0;
    void Start()
    {
        StartCoroutine(IncreaseHealthBar());
        StopCoroutine(IncreaseHealthBar());
    }

    IEnumerator IncreaseHealthBar()
    {
        while (x < 1150)
        {
            x += 1150 * Time.deltaTime / 3;
            GetComponent<RectTransform>().sizeDelta = new Vector2(x, 25);
            yield return null;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(1150, 25);
    }
}
