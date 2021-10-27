using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ConditionNode))]
public class ConditionNodeEditor : Editor
{
    public System.Object obj;
    public override void OnInspectorGUI()
    {
        var node = target as ConditionNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Condition Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("Choose the variable you want to compare then enter a value, or choose from the blackboard a value you want to compare it with.", wrap);
        node.variableIndex = EditorGUILayout.Popup(new GUIContent("Type"), node.variableIndex, Enum.GetNames(typeof(VariableType)));

        if (node.variableIndex < 5)
        {
            if (node.variableIndex != 2)
                node.enableKey = EditorGUILayout.Toggle("Compare Blackboard Value", node.enableKey);
            node.index = EditorGUILayout.Popup(new GUIContent("Keybind"), node.index, node.keybinds.ToArray());
        }

        if (node.variableIndex == 2 || node.variableIndex == 4)
        {
            node.operatorList = new List<string>() { "==", "!=" };
            node.operatorIndex = EditorGUILayout.Popup(new GUIContent("Operator"), node.operatorIndex, node.operatorList.ToArray());
        }
        else if (node.variableIndex == 3)
        {
            node.operatorList = new List<string>() { "==", "!=", "Contains" };
            node.operatorIndex = EditorGUILayout.Popup(new GUIContent("Operator"), node.operatorIndex, node.operatorList.ToArray());
        }
        else if (node.variableIndex < 5)
        {
            node.operatorList = new List<string>() { "==", "!=", "<", "<=", ">", ">=" };
            node.operatorIndex = EditorGUILayout.Popup(new GUIContent("Operator"), node.operatorIndex, node.operatorList.ToArray());
        }

        if (node.enableKey)
        {
            if (node.variableIndex < 2)
            {
                node.indexCopy = EditorGUILayout.Popup(new GUIContent("Compare with"), node.indexCopy, node.keybindsCopy.ToArray());
                node.comparePercent = EditorGUILayout.Toggle("Compare Percentage", node.comparePercent);
                if (node.comparePercent)
                    node.floatValue = EditorGUILayout.FloatField(node.keybindCopy + " (%)", node.floatValue);
            }
            else if (node.variableIndex == 2)
                node.boolIndex = EditorGUILayout.Popup("Compare New Value", node.boolIndex, new string[] { "True", "False" });
            else if (node.variableIndex == 5)
                EditorGUILayout.LabelField("There is no reason to compare GameObjects, This will return FAILURE", wrap);
            else if (node.variableIndex == 6)
                EditorGUILayout.LabelField("There is no reason to compare Delegates, This will return FAILURE", wrap);
            else
                node.indexCopy = EditorGUILayout.Popup(new GUIContent("Compare with"), node.indexCopy, node.keybindsCopy.ToArray());
        }
        else
        {
            switch (node.variableIndex)
            {
                case 0:
                    node.floatValue = EditorGUILayout.FloatField("Compare New Value", node.floatValue);
                    break;
                case 1:
                    node.floatValue = EditorGUILayout.FloatField("Compare New Value", node.floatValue);
                    break;
                case 2:
                    node.boolIndex = EditorGUILayout.Popup("Compare New Value", node.boolIndex, new string[] { "True", "False" });
                    break;
                case 3:
                    node.stringValue = EditorGUILayout.TextField("Compare New Value", node.stringValue);
                    break;
                case 4:
                    node.vector2Value = EditorGUILayout.Vector2Field("Compare New Value", node.vector2Value);
                    break;
                case 5:
                    EditorGUILayout.LabelField("There is no reason to compare GameObjects, This will return FAILURE", wrap);
                    break;
                case 6:
                    EditorGUILayout.LabelField("There is no reason to compare Delegates, This will return FAILURE", wrap);
                    break;
            }
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(RepeatNode))]
public class RepeatNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as RepeatNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Repeat Node", font);
        EditorGUILayout.Separator();

        node.loopInfinitely = EditorGUILayout.Toggle("Loop Infinitely", node.loopInfinitely);
        if (node.loopInfinitely == false)
        {
            EditorGUI.indentLevel++;
            node.enableKeyloop = EditorGUILayout.Toggle("Value from blackboard", node.enableKeyloop);
            if (node.enableKeyloop)
            {
                EditorGUI.indentLevel++;

                GUIContent content = new GUIContent("Keybind");
                node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUI.indentLevel++;
                node.loopCount = EditorGUILayout.IntField("Loop Amount", node.loopCount);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(InvertNode))]
public class InvertNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as InvertNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Invert Node", font);
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}