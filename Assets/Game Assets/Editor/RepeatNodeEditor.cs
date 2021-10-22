using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RepeatNode))]
public class RepeatNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var repeatNode = target as RepeatNode;

        repeatNode.loopInfinitely = EditorGUILayout.Toggle("Loop Infinitely", repeatNode.loopInfinitely);
        if (repeatNode.loopInfinitely == false)
        {
            repeatNode.loopCount = EditorGUILayout.IntField("Loop Amount", repeatNode.loopCount);
        }
    }
}
