using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : DecoratorNode
{
    public float floatValue;
    public float percent;
    public string stringValue;
    public Vector2 vector2Value;

    public int prevCount;

    public int prevIndex;
    public float keybindFloat;
    public string keybindString;
    public Vector2 keybindVector2;

    public int boolIndex;
    public List<string> boolList = new List<string>() { "True", "False" };

    public int operatorIndex;
    public List<string> operatorList = new List<string>();

    public int compareIndex;
    public string compareName;
    public int compareCount;
    public float compareFloat;
    public string compareString;
    public Vector2 compareVector2;
    public List<string> compareList = new List<string>();

    public bool compareWithBlackboardKey;
    public bool comparePercent;

    public string[] chosenKeybind;  
    public string[] chosenCompare;

    bool success;
    protected override void OnStart()
    {
        success = false;
        bool isNumeric = false;
        bool isAlpha = false;
        bool dummy = false;

        if (chosenKeybind[1].Contains("Boolean"))
        {
            if (boolIndex == 0)
                    success = blackboard.booleans.GetValue(chosenKeybind[0]);
            else
                    success = !blackboard.booleans.GetValue(chosenKeybind[0]);
        }
        else
        {
            chosenCompare = compareList[compareIndex].Split(new string[] { " (" }, System.StringSplitOptions.None);
            FindValues(chosenKeybind, ref keybindFloat, ref keybindString, ref keybindVector2, ref isNumeric, ref isAlpha);
            if (compareWithBlackboardKey)
                FindValues(chosenCompare, ref floatValue, ref stringValue, ref vector2Value, ref dummy, ref dummy);
            if (isNumeric)
            {
                if (!comparePercent)
                    percent = 100;
                success = NumericComparison(keybindFloat, floatValue * percent / 100);
            }
            else if (isAlpha)
                success = AlphaComparison(keybindString, stringValue);
            else
                success = Vector2Comparison(keybindVector2, vector2Value);
        }
    }
    protected override State OnUpdate()
    {
        if (success)
            child.Update();
        return success ? child.state : State.FAILURE;
    }
    public override void OnBeforeSerialize()
    {
        if (prevIndex != index)
        {
            prevIndex = index;
            boolIndex = 0;
            operatorIndex = 0;
            compareIndex = 0;
        }

        keybinds.Clear();
        operatorList.Clear();
        compareList.Clear();

        chosenKeybind = new string[] { "", "" };
        operatorList = new List<string>() { "==", "!=", "<", "<=", ">", ">=" };

        foreach (Key<int> key in blackboard.integers.keys)
            keybinds.Add(key.name + " (Integer Key)");
        foreach (Key<float> key in blackboard.floats.keys)
            keybinds.Add(key.name + " (Float Key)");
        foreach (Key<bool> key in blackboard.booleans.keys)
            keybinds.Add(key.name + " (Boolean Key)");
        foreach (Key<string> key in blackboard.strings.keys)
            keybinds.Add(key.name + " (String Key)");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key)");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key [X])");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key [Y])");

        if (prevCount != keybinds.Count)
        {
            prevCount = keybinds.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < keybinds.Count && !found; i++)
                if (keybinds[i] == keybind)
                    found = true;
            if (found)
                index = i - 1;
            else
                index = 0;
            prevIndex = index;
        }
        if (keybinds.Count > 0)
        {
            chosenKeybind = keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None);
            keybind = keybinds[index];
            if (chosenKeybind[1].Contains("Integer") ||
                chosenKeybind[1].Contains("Float") ||
                chosenKeybind[1].Contains("[X]") ||
                chosenKeybind[1].Contains("[Y]"))
            {
                foreach (Key<int> key in blackboard.integers.keys)
                    compareList.Add(key.name + " (Integer Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    compareList.Add(key.name + " (Float Key)");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    compareList.Add(key.name + " (Vector2 Key [X])");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    compareList.Add(key.name + " (Vector2 Key [Y])");

                operatorList = new List<string>() { "==", "!=", "<", "<=", ">", ">=" };
            }
            else if (chosenKeybind[1].Contains("String"))
            {
                foreach (Key<string> key in blackboard.strings.keys)
                    compareList.Add(key.name + " (String Key)");
                operatorList = new List<string>() { "==", "!=", "Contains" };
            }
            else if (chosenKeybind[1].Contains("Vector2 Key)"))
            {
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    compareList.Add(key.name + " (Vector2 Key)");
                operatorList = new List<string>() { "==", "!=" };
            }
            else
            {
                operatorList = new List<string>() { "==" };
            }
        }
        if (compareCount != compareList.Count)
        {
            compareCount = compareList.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < compareList.Count && !found; i++)
                if (compareList[i] == compareName)
                    found = true;
            if (found)
                compareIndex = i - 1;
            else
                compareIndex = 0;
        }
        if (compareCount > 0)
            compareName = compareList[compareIndex];
    }
    public bool NumericComparison(float op1, float op2)
    {
        switch (operatorIndex)
        {
            case 0:
                return op1 == op2;
            case 1:
                return op1 != op2;
            case 2:
                return op1 < op2;
            case 3:
                return op1 <= op2;
            case 4:
                return op1 > op2;
            case 5:
                return op1 >= op2;
        }
        return false;
    }
    bool AlphaComparison(string str1, string str2)
    {
        switch (operatorIndex)
        {
            case 0:
                return str1 == str2;
            case 1:
                return str1 != str2;
            case 2:
                return str1.Contains(str2);
        }
        return false;
    }

    bool Vector2Comparison(Vector2 vect1, Vector2 vect2)
    {
        switch (operatorIndex)
        {
            case 0:
                return vect1 == vect2;
            case 1:
                return vect1 != vect2;
        }
        return false;
    }
    void FindValues(string[] chosen, ref float num, ref string str, ref Vector2 vect, ref bool numeric, ref bool stringg)
    {
        if (chosen[1].Contains("Integer"))
        {
            num = blackboard.integers.GetValue(chosen[0]);
            numeric = true;
        }
        else if (chosen[1].Contains("Float"))
        {
            num = blackboard.floats.GetValue(chosen[0]);
            numeric = true;
            stringg = false;
        }
        else if (chosen[1].Contains("[X]"))
        {
            num = blackboard.vector2s.GetValue(chosen[0]).x;
            numeric = true;
            stringg = false;
        }
        else if (chosen[1].Contains("[Y]"))
        {
            num = blackboard.vector2s.GetValue(chosen[0]).y;
            numeric = true;
            stringg = false;
        }
        else if (chosen[1].Contains("String"))
        {
            str = blackboard.strings.GetValue(chosen[0]);
            stringg = true;
            numeric = false;
        }
        else if (chosen[1].Contains("Vector2 Key)"))
        {
            vect = blackboard.vector2s.GetValue(chosen[0]);
            numeric = false;
            stringg = false;
        }
    }
}
