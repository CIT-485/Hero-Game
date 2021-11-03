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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will perform each child's update in order from left to right, until it finds a child that returns failure", wrap);


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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will perform each children's update in order from left to right, until it finds a child that returns successful", wrap);


        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }

}



[CustomEditor(typeof(ParallelSequenceNode))]
public class ParallelSequenceNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as ParallelSequenceNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Parallel Sequence Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will perform each children's update in order from left to right, until it finds a child that returns failure all within one update", wrap);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(ParallelSelectorNode))]
public class ParallelSelectorNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as ParallelSelectorNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Parallel Sequence Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will perform each children's update in order from left to right, until it finds a child that returns successful all within one update", wrap);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        base.OnInspectorGUI();
    }
}