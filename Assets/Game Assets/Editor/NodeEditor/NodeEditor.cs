using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RootNode))]
public class RootNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as RootNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = Color.white;
        font.normal.background = tex;
        EditorGUILayout.LabelField("Root Node", font);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}
public static class Texture2DExtensions
{
    public static void SetColor(this Texture2D tex2, Color32 color)
    {


        var fillColorArray = tex2.GetPixels32();

        for (var i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = color;
        }

        tex2.SetPixels32(fillColorArray);

        tex2.Apply();
    }
}
