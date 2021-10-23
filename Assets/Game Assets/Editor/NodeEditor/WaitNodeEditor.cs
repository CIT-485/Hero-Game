using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaitNode))]
public class WaitNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as WaitNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = Color.white;
        font.normal.background = tex;
        EditorGUILayout.LabelField("Wait Node", font);
        EditorGUILayout.Separator();
        node.index = EditorGUILayout.IntField("Loop Amount", node.index);

        node.enableKey = EditorGUILayout.Toggle("Value from blackboard", node.enableKey);
        if (node.enableKey)
        {
            EditorGUI.indentLevel++;
            node.keybind = EditorGUILayout.TextField("Keybind (Int)", node.keybind);

            GUIContent content = new GUIContent("Keybind");
            node.index = EditorGUILayout.Popup(content, node.index, node.blackboard.keys.ToArray());

            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            node.duration = EditorGUILayout.FloatField("Loop Amount", node.duration);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}