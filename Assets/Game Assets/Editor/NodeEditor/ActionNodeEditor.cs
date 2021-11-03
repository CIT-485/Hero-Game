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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will wait for a given period of time in seconds", wrap);


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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will write a message to the unity console", wrap);

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

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);
        EditorGUILayout.LabelField("This will perform a chosen delegate. Be sure to have a delegate in your blackboard", wrap);

        GUIContent content = new GUIContent("Keybind");
        node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(SetValueNode))]
public class SetValueNodeEditor : Editor
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

        GUIStyle note = new GUIStyle();
        note.wordWrap = true;
        note.normal.textColor = new Color32(200, 200, 200, 255);
        note.fontSize = 10;

        EditorGUILayout.LabelField("Choose a key to change the value of. You can enter an value yourself, or copy and value from another key", wrap);
        EditorGUILayout.Separator();

        // display option to use blackboard key
        if (node.chosenKeybind[1].Contains("Boolean"))
        {
            GUI.enabled = false;
            node.useBlackboardKey = false;
            node.useBlackboardKey = EditorGUILayout.Toggle("Copy From Blackboard", node.useBlackboardKey);
            GUI.enabled = true;
        }
        else
            node.useBlackboardKey = EditorGUILayout.Toggle("Copy From Blackboard", node.useBlackboardKey);

        // display keybind
        node.index = EditorGUILayout.Popup(new GUIContent("Keybind"), node.index, node.keybinds.ToArray());

        // display value
        if (node.useBlackboardKey)
        {
            node.newIndex = EditorGUILayout.Popup(new GUIContent("New Value"), node.newIndex, node.newList.ToArray());
            if (node.chosenKeybind[1].Contains("Integer"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Note: Choosing an integer will round the number to fit the parameters", note);
                EditorGUI.indentLevel--;
            }
        }
        else
        {
            if (node.IsNumeric())
                if (node.chosenKeybind[1].Contains("Integer"))
                    node.intValue = EditorGUILayout.IntField("New Value", node.intValue);
                else
                    node.floatValue = EditorGUILayout.FloatField("New Value", node.floatValue);
            else if (node.chosenKeybind[1].Contains("String"))
                node.stringValue = EditorGUILayout.TextField("New Value", node.stringValue);
            else if (node.chosenKeybind[1].Contains("Boolean"))
            {
                node.invert = EditorGUILayout.Toggle("Invert boolean", node.invert);
                if (!node.invert)
                    node.boolIndex = EditorGUILayout.Popup("New Value", node.boolIndex, node.boolList.ToArray());
            }
            else if (node.chosenKeybind[1].Contains("2 Key)"))
                node.vector2Value = EditorGUILayout.Vector2Field("New Value", node.vector2Value);
            else
            {
                obj = EditorGUILayout.ObjectField("New Value", node.objValue, typeof(GameObject), true);
                node.objValue = obj as GameObject;
            }
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(RNGNode))]
public class RNGNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as RNGNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Random Number Generator Node", font);
        EditorGUILayout.Separator();

        GUIStyle wrap = new GUIStyle();
        wrap.wordWrap = true;
        wrap.normal.textColor = new Color32(200, 200, 200, 255);

        GUIStyle note = new GUIStyle();
        note.wordWrap = true;
        note.normal.textColor = new Color32(200, 200, 200, 255);
        note.fontSize = 10;

        EditorGUILayout.LabelField("Choose the type of the variable you want to have generate a random number, then specify the range. (Lower bound is inclusive; Upper bound is exclusive)", wrap);
        EditorGUILayout.Separator();

        GUIContent content = new GUIContent("Keybind");
        node.index = EditorGUILayout.Popup(content, node.index, node.keybinds.ToArray());

        node.lowerBound = EditorGUILayout.FloatField("Lower Bound", node.lowerBound);
        node.upperBound = EditorGUILayout.FloatField("Upper Bound", node.upperBound);

        if (node.chosenKeybind[1].Contains("Integer"))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Note: Choosing an integer will round the number to fit the parameters", note);
            EditorGUI.indentLevel--;
        }

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

        GUIStyle note = new GUIStyle();
        note.wordWrap = true;
        note.normal.textColor = new Color32(200, 200, 200, 255);
        note.fontSize = 10;

        EditorGUILayout.LabelField("Choose two operands and an operator, then a key to store to results in. Toggle boolean below to have to option enter an value for the second operand instead.", wrap);
        EditorGUILayout.Separator();

        node.getBlackboardKey = EditorGUILayout.Toggle("Get Blackboard Value", node.getBlackboardKey);

        // display Operand One
        node.operandOneIndex = EditorGUILayout.Popup("First Operand", node.operandOneIndex, node.operandOneList.ToArray());

        // display Operator
        node.operatorList = new List<string>() { "+", "-", "~", "€ (division)" };
        node.operatorIndex = EditorGUILayout.Popup("Operator", node.operatorIndex, node.operatorList.ToArray());

        // display Operand Two
        if (node.getBlackboardKey)
            node.operandTwoIndex = EditorGUILayout.Popup("Second Operand", node.operandTwoIndex, node.operandTwoList.ToArray());
        else
            node.enteredValue = EditorGUILayout.FloatField("Second Operand", node.enteredValue);

        EditorGUILayout.Separator();

        // display Results
        node.resultIndex = EditorGUILayout.Popup("Save Results to", node.resultIndex, node.resultList.ToArray());
        if (node.resultList.Count > 0)
        {
            if (node.resultList[node.resultIndex].Split(new string[] { " (" }, System.StringSplitOptions.None)[1].Contains("Integer"))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Note: Choosing an integer as the save location will round the number to fit the parameters", note);
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(AnimationNode))]
public class AnimationNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var node = target as AnimationNode;

        GUIStyle font = new GUIStyle();
        Texture2D tex = new Texture2D(2, 2);
        tex.SetColor(new Color32(37, 37, 37, 255));
        font.fontSize = 16;
        font.normal.textColor = new Color32(200, 200, 200, 255);
        font.normal.background = tex;
        EditorGUILayout.LabelField("Animation Node", font);
        EditorGUILayout.Separator();

        node.index = EditorGUILayout.Popup("Animator", node.index, node.keybinds.ToArray());

        node.varIndex = EditorGUILayout.Popup("Variable Type", node.varIndex, node.variables.ToArray()); ;

        node.varName = EditorGUILayout.TextField("Variable Name", node.varName);

        switch (node.varIndex)
        {
            case 0:
                node.floatValue = EditorGUILayout.FloatField("New Value", node.floatValue);
                break;
            case 1:
                node.intValue = EditorGUILayout.IntField("New Value", node.intValue);
                break;
            case 2:
                node.boolIndex = EditorGUILayout.Popup("New Value", node.boolIndex, node.boolList.ToArray());
                break;
        }

        EditorGUILayout.LabelField("Description");
        node.description = EditorGUILayout.TextArea(node.description, GUILayout.MaxHeight(75));

        //base.OnInspectorGUI();
    }
}