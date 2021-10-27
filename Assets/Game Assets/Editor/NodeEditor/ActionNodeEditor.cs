using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaitNode))]
public class WaitNodeEditor : Editor
{
    List<string> list = new List<string>();
    public override void OnInspectorGUI()
    {
        var node = target as WaitNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Wait Node", font);
        EditorGUILayout.Separator();

        node.enableKey = EditorGUILayout.Toggle("Get Blackboard Value", node.enableKey);
        if (node.enableKey)
        {
            EditorGUI.indentLevel++;

            GUIContent content = new GUIContent("Keybind");
            node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            node.duration = EditorGUILayout.FloatField("Duration (seconds)", node.duration);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(DebugLogNode))]
public class DebugLogNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as DebugLogNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Debug Log Node", font);
        EditorGUILayout.Separator();

        node.enableKey = EditorGUILayout.Toggle("Get Blackboard Value", node.enableKey);
        if (node.enableKey)
        {
            EditorGUI.indentLevel++;

            GUIContent content = new GUIContent("Keybind");
            node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

            EditorGUI.indentLevel--;
        }
        else
        {
            EditorGUI.indentLevel++;
            node.message = EditorGUILayout.TextField("Message", node.message);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(DelegateNode))]
public class DelegateNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as DelegateNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Delegate Node", font);
        EditorGUILayout.Separator();

        GUIContent content = new GUIContent("Keybind");
        node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}




[CustomEditor(typeof(SetValueNode))]
public class BlackboardEditNodeEditor : Editor
{
    public System.Object obj;
    public override void OnInspectorGUI()
    {
        var node = target as SetValueNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Set Value Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("Choose the type of the variable you want to edit then pick the variable itself. You can then assign a value or copy a value from the blackboard from another key", wrap);
        node.variableIndex = EditorGUILayout.Popup(new GUIContent("Type"), node.variableIndex, Enum.GetNames(typeof(VariableType)));


        node.index = EditorGUILayout.Popup(new GUIContent("Keybind"), node.index, node.keybinds.ToArray());

        EditorGUI.indentLevel++;
        node.enableKey = EditorGUILayout.Toggle("Copy Blackboard Value", node.enableKey);
        EditorGUI.indentLevel--;
        if (node.enableKey)
        {
            node.indexCopy = EditorGUILayout.Popup(new GUIContent("Copy From"), node.indexCopy, node.keybindsCopy.ToArray());
        }
        else
        {
            switch (node.variableIndex)
            {
                case 0:
                    node.intValue = EditorGUILayout.IntField("New Value", node.intValue);
                    break;
                case 1:
                    node.floatValue = EditorGUILayout.FloatField("New Value", node.floatValue);
                    break;
                case 2:
                    node.enableInvert = EditorGUILayout.Toggle("Invert Boolean", node.enableInvert);
                    if (!node.enableInvert)
                        node.boolIndex = EditorGUILayout.Popup("New Value", node.boolIndex, new string[] { "True", "False" });
                    break;
                case 3:
                    node.stringValue = EditorGUILayout.TextField("New Value", node.stringValue);
                    break;
                case 4:
                    node.vector2Value = EditorGUILayout.Vector2Field("New Value", node.vector2Value);
                    break;
                case 5:
                    obj = EditorGUILayout.ObjectField("New Value", node.gameObjectValue, typeof(GameObject), true);
                    node.gameObjectValue = obj as GameObject;
                    break;
                case 6:
                    EditorGUILayout.LabelField("You can't assign an abstract delegate. This will return FAILURE!", wrap);
                    break;
            }
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(GenerateRandomNumeralNode))]
public class GenerateRandomNumeralNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as GenerateRandomNumeralNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Debug Log Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("Choose the type of the variable you want to have generate a random number, then specify the range. (Lower bound is inclusive; Upper bound is exclusive)", wrap);
        node.variableIndex = EditorGUILayout.Popup(new GUIContent("Type"), node.variableIndex, node.variableTypes.ToArray());

        GUIContent content = new GUIContent("Keybind");
        node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

        node.lowerBound = EditorGUILayout.FloatField("Lower Bound", node.lowerBound);
        node.upperBound = EditorGUILayout.FloatField("Upper Bound", node.upperBound);

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}




[CustomEditor(typeof(ArithmeticNode))]
public class ArithmeticNodeEditor : Editor
{
    public System.Object obj;
    public override void OnInspectorGUI()
    {
        var node = target as ArithmeticNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Arithmetic Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("Choose the variable you want to compare then enter a value, or choose from the blackboard a value you want to compare it with.", wrap);
        node.variableIndex = EditorGUILayout.Popup(new GUIContent("Type"), node.variableIndex, node.variableTypes.ToArray());

        node.enableKey = EditorGUILayout.Toggle("Use Blackboard Value", node.enableKey);
        node.index = EditorGUILayout.Popup(new GUIContent("Operand 1"), node.index, node.keybinds.ToArray());
        
        node.operatorList = new List<string>() { "+", "-", "Å~", "ÅÄ (division)" };
        node.operatorIndex = EditorGUILayout.Popup(new GUIContent("Operator"), node.operatorIndex, node.operatorList.ToArray());

        if (node.enableKey)
            node.indexCopy = EditorGUILayout.Popup(new GUIContent("Operand 2"), node.indexCopy, node.keybindsCopy.ToArray());
        else
            node.floatValue = EditorGUILayout.FloatField("Operand 2", node.floatValue);

        node.resultIndex = EditorGUILayout.Popup("Save Results to", node.resultIndex, node.resultList.ToArray());

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}
