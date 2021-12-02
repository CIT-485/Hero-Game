using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatNumber : MonoBehaviour
{
    public TMP_Text display;
    public int number;

    public void Update()
    {
        display.text = number.ToString();
    }
}