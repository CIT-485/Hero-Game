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
        EditorGUILayout.LabelField("Choose two operands and an operator. Once the resulting boolean is true, then the node will perform its child's update function continuously.", wrap);

        EditorGUILayout.Separator();

        if (node.chosenKeybind[1].Contains("Boolean"))
        {
            GUI.enabled = false;
            node.compareWithBlackboardKey = EditorGUILayout.Toggle("Compare Blackboard Value", false);
            GUI.enabled = true;
        }
        else
            node.compareWithBlackboardKey = EditorGUILayout.Toggle("Compare Blackboard Value", node.compareWithBlackboardKey);

        // Greys out percent toggle if the blackboard key is NOT a numeral
        if ((node.chosenKeybind[1].Contains("Integer") ||
            node.chosenKeybind[1].Contains("Float") ||
            node.chosenKeybind[1].Contains("[X]") ||
            node.chosenKeybind[1].Contains("[Y]")) &&
            node.compareWithBlackboardKey)
            node.comparePercent = EditorGUILayout.Toggle("Compare Percentage(%)", node.comparePercent);
        else
        {
            GUI.enabled = false;
            node.comparePercent = EditorGUILayout.Toggle("Compare Percentage(%)", false);
            GUI.enabled = true;
        }

        // display blackboard key
        node.index = EditorGUILayout.Popup("Operand One", node.index, node.keybinds.ToArray());

        // display associated operators
        if (node.chosenKeybind[1].Contains("Boolean"))
        {
            GUI.enabled = false;
            node.operatorIndex = 0;
            node.operatorIndex = EditorGUILayout.Popup("Operator", node.operatorIndex, node.operatorList.ToArray());
            GUI.enabled = true;
        }
        else
            node.operatorIndex = EditorGUILayout.Popup("Operator", node.operatorIndex, node.operatorList.ToArray());

        // display comparison
        if (node.compareWithBlackboardKey)
        {
            node.compareIndex = EditorGUILayout.Popup("Operand Two", node.compareIndex, node.compareList.ToArray());
        }
        else
        {
            if (node.chosenKeybind[1].Contains("Integer") ||
               node.chosenKeybind[1].Contains("Float") ||
               node.chosenKeybind[1].Contains("[X]") ||
               node.chosenKeybind[1].Contains("[Y]"))
                node.floatValue = EditorGUILayout.FloatField("Operand Two", node.floatValue);
            else if (node.chosenKeybind[1].Contains("Boolean"))
                node.boolIndex = EditorGUILayout.Popup("Operand Two", node.boolIndex, node.boolList.ToArray());
            else if (node.chosenKeybind[1].Contains("String"))
                node.stringValue = EditorGUILayout.TextField("Operand Two", node.stringValue);
            else if (node.chosenKeybind[1].Contains("2 Key)"))
                node.vector2Value = EditorGUILayout.Vector2Field("Operand Two", node.vector2Value);
        }
        if (node.comparePercent && node.compareWithBlackboardKey)
            node.percent = EditorGUILayout.FloatField("% of Operand Two", node.percent);

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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will repeat its child's update status until a it hits its max loop count.", wrap);

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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will return the inverted state of it's child. For example; if the child returns successful, then this node will return as failure and vice-versa", wrap);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(FailureNode))]
public class FailureNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as FailureNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Failure Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will always return as failure, regardless if the child was successful or not.", wrap);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(SuccessNode))]
public class SuccessNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as SuccessNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Success Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will always return as successful, regardless if the child was successful or not.", wrap);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}