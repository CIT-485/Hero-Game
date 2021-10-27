using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SequenceNode))]
public class SequenceNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as SequenceNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Sequence Node", font);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(SelectorNode))]
public class SelectorNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as SelectorNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Selector Node", font);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }

}



[CustomEditor(typeof(ParallelNode))]
public class ParallelNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as ParallelNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Parallel Node", font);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }
}