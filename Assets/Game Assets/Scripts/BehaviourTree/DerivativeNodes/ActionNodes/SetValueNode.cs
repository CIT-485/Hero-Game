using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValueNode : ActionNode
{
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public string stringValue;
    public Vector2 vector2Value;
    public GameObject objValue;

    public int prevIndex;
    public int prevCount;

    public int boolIndex;
    public List<string> boolList = new List<string>() { "True", "False" };

    public int newIndex;
    public List<string> newList = new List<string>();

    public bool invert;
    public bool useBlackboardKey;

    public string[] chosenKeybind;
    public string[] chosenNew;

    protected override void OnStart()
    {
        Key<int> keybindInt = new Key<int>();
        Key<float> keybindFloat = new Key<float>();
        Key<string> keybindString = new Key<string>();
        Key<Vector2> keybindVect2 = new Key<Vector2>();
        Key<GameObject> keybindObj = new Key<GameObject>();

        bool isX = false;

        float newFloat = 0;
        string newString = "";
        Vector2 newVect2 = Vector2.zero;
        GameObject newObj = null;

        if (boolIndex == 0)
            boolValue = true;
        else
            boolValue = false;

        if (chosenKeybind[1].Contains("Integer"))
            keybindInt = blackboard.integers.GetKey(chosenKeybind[0]);
        else if (chosenKeybind[1].Contains("Float"))
            keybindFloat = blackboard.floats.GetKey(chosenKeybind[0]);
        else if (chosenKeybind[1].Contains("[X]"))
        {
            keybindVect2 = blackboard.vector2s.GetKey(chosenKeybind[0]);
            isX = true;
        }
        else if (chosenKeybind[1].Contains("[Y]"))
            keybindVect2 = blackboard.vector2s.GetKey(chosenKeybind[0]);
        else if (chosenKeybind[1].Contains("String"))
            keybindString = blackboard.strings.GetKey(chosenKeybind[0]);
        else if (chosenKeybind[1].Contains("Vector2 Key)"))
            keybindVect2 = blackboard.vector2s.GetKey(chosenKeybind[0]);
        else if (chosenKeybind[1].Contains("Game"))
            keybindObj = blackboard.gameObjects.GetKey(chosenKeybind[0]);
        if (useBlackboardKey)
        {
            chosenNew = newList[newIndex].Split(new string[] { " (" }, System.StringSplitOptions.None);
            FindValues(chosenNew, ref newFloat, ref newString, ref newVect2, ref newObj);
        }
        else
        {
            if (chosenKeybind[1].Contains("Integer"))
                newFloat = intValue;
            else
                newFloat = floatValue;
            newString = stringValue;
            newVect2 = vector2Value;
            newObj = objValue;
        }
        if (IsNumeric())
        {
            if (isX)
                keybindVect2.value.x = newFloat;
            else
                keybindVect2.value.y = newFloat;
        }
        keybindInt.value = (int)newFloat;
        keybindFloat.value = newFloat;
        if (chosenKeybind[1].Contains("Boolean"))
            blackboard.booleans.GetValue(chosenKeybind[0]) = boolValue;
        keybindString.value = newString;
        keybindVect2.value = newVect2;
        keybindObj.value = newObj;
    }
    protected override State OnUpdate()
    {
        return State.SUCCESS;
    }
    public override void OnBeforeSerialize()
    {
        if (prevIndex != index)
        {
            prevIndex = index;
            newIndex = 0;
            boolIndex = 0;
        }

        keybinds.Clear();
        newList.Clear();

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
        foreach (Key<GameObject> key in blackboard.gameObjects.keys)
            keybinds.Add(key.name + " (GameObject Key)");

        chosenKeybind = new string[] { "", "" };

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
            if (IsNumeric())
            {
                foreach (Key<int> key in blackboard.integers.keys)
                    newList.Add(key.name + " (Integer Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    newList.Add(key.name + " (Float Key)");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    newList.Add(key.name + " (Vector2 Key [X])");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    newList.Add(key.name + " (Vector2 Key [Y])");
            }
            else if (chosenKeybind[1].Contains("String"))
                foreach (Key<string> key in blackboard.strings.keys)
                    newList.Add(key.name + " (String Key)");
            else if (chosenKeybind[1].Contains("2 Key)"))
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    newList.Add(key.name + " (Vector2 Key)");
            else
                foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                    newList.Add(key.name + " (GameObject Key)");
        }
    }
    public bool IsNumeric()
    {
        if (chosenKeybind[1].Contains("Integer") ||
               chosenKeybind[1].Contains("Float") ||
               chosenKeybind[1].Contains("[X]") ||
               chosenKeybind[1].Contains("[Y]"))
            return true;
        else
            return false;
    }
    void FindValues(string[] chosen, ref float num, ref string str, ref Vector2 vect, ref GameObject obj)
    {
        if (chosen[1].Contains("Integer"))
            num = blackboard.integers.GetValue(chosen[0]);
        else if (chosen[1].Contains("Float"))
            num = blackboard.floats.GetValue(chosen[0]);
        else if (chosen[1].Contains("[X]"))
            num = blackboard.vector2s.GetValue(chosen[0]).x;
        else if (chosen[1].Contains("[Y]"))
            num = blackboard.vector2s.GetValue(chosen[0]).y;
        else if (chosen[1].Contains("String"))
            str = blackboard.strings.GetValue(chosen[0]);
        else if (chosen[1].Contains("Vector2 Key)"))
            vect = blackboard.vector2s.GetValue(chosen[0]);
        else if (chosen[1].Contains("Game"))
            obj = blackboard.gameObjects.GetValue(chosen[0]);
    }
}
