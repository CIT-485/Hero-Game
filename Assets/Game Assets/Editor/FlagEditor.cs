using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Flag))]
public class FlagEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Flag flag = (Flag)target;

        GUILayout.Label("Choose specific tags for the flag to trigger");
        GUIContent content = new GUIContent("Specify Tag(s)");
        flag.tags = UnityEditorInternal.InternalEditorUtility.tags;
        flag.index = EditorGUILayout.MaskField(content, flag.index, flag.tags);
        base.OnInspectorGUI();
    }
}
