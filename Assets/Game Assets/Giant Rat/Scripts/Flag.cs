using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Flag))]
public class FlagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Flag flag = (Flag)target;

        GUILayout.Label("Choose specific tags for the flag to trigger");
        GUIContent content = new GUIContent("Specify Tag(s)");
        flag.index = EditorGUILayout.MaskField(content, flag.index, UnityEditorInternal.InternalEditorUtility.tags);
        base.OnInspectorGUI();
    }
}

public class Flag : MonoBehaviour
{
    [HideInInspector]
    public int      index = 0;
    int             prevIndex = 0;
    List<string>    selectedTags = new List<string>();
    public bool     flagged;
    public bool     paused;
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
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.tags.Length; i++)
        {
            int layer = 1 << i;
            if ((index & layer) != 0)
            {
                selectedTags.Add(UnityEditorInternal.InternalEditorUtility.tags[i]);
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
