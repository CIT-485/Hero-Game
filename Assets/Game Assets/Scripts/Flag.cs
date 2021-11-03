using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [HideInInspector]
    public int              index = 0;
    int                     prevIndex = 0;
    [HideInInspector]
    public string[]         tags = new string[] { };
    List<string>            selectedTags = new List<string>();
    public bool             flagged;
    public bool             paused;

    private void Start()
    {
        prevIndex = index;
        UpdateSelection();
    }
    private void Update()
    {
        if (prevIndex != index)
        {
            prevIndex = index;
            UpdateSelection();
        }
    }
    private void UpdateSelection()
    {
        selectedTags.Clear();
        for (int i = 0; i < tags.Length; i++)
        {
            int layer = 1 << i;
            if ((index & layer) != 0)
            {
                selectedTags.Add(tags[i]);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!paused)
        {
            foreach (string tag in selectedTags)
            {
                if (collision.tag == tag)
                {
                    flagged = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (string tag in selectedTags)
        {
            if (collision.tag == tag)
            {
                flagged = false;
            }
        }
    }
}
