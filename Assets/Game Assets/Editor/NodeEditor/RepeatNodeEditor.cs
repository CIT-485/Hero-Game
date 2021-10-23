using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RepeatNode))]
public class RepeatNodeEditor : Editor
{
    public SerializedProperty descriptionProp;
    void OnEnable()
    {
        descriptionProp = serializedObject.FindProperty("description");
    }
    public override void OnInspectorGUI()
    {
        var repeatNode = target as RepeatNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = Color.white;
        font.normal.background = tex;
        EditorGUILayout.LabelField("Repeat Node", font);
        EditorGUILayout.Separator();

        repeatNode.loopInfinitely = EditorGUILayout.Toggle("Loop Infinitely", repeatNode.loopInfinitely);
        if (repeatNode.loopInfinitely == false)
        {
            EditorGUI.indentLevel++;
            repeatNode.enableKeyloop = EditorGUILayout.Toggle("Value from blackboard", repeatNode.enableKeyloop);
            if (repeatNode.enableKeyloop)
            {
                EditorGUI.indentLevel++;
                repeatNode.keybind = EditorGUILayout.TextField("Keybind (Int/Float)", repeatNode.keybind);
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUI.indentLevel++;
                repeatNode.loopCount = EditorGUILayout.IntField("Loop Amount", repeatNode.loopCount);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Description");
        repeatNode.description = EditorGUILayout.TextArea(repeatNode.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}
