using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEMO : MonoBehaviour
{
    public Image img;
    public KeyCode key;
    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            img.color = Color.green;
        }
        else if (Input.GetKeyUp(key))
        {
            img.color = Color.white;
        }
    }
}
