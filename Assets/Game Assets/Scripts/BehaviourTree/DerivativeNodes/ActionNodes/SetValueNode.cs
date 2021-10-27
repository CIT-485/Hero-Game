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
    public GameObject gameObjectValue;

    public int variableIndex = 0;
    public int prevVariableIndex = 0;

    public List<string> keybindsCopy = new List<string>();
    public int indexCopy;
    public string keybindCopy;

    public bool enableKey = false;
    public bool enableInvert = false;
    public int boolIndex;

    protected override void OnStart()
    {
        if (boolIndex == 0)
            boolValue = true;
        else
            boolValue = false;
    }
    protected override State OnUpdate()
    {
        if (enableKey)
        {
            switch (variableIndex)
            {
                case 0:
                    blackboard.integers.Find(keybind) = blackboard.integers.Find(keybindCopy);
                    break;
                case 1:
                    blackboard.floats.Find(keybind) = blackboard.floats.Find(keybindCopy);
                    break;
                case 2:
                    blackboard.booleans.Find(keybind) = blackboard.booleans.Find(keybindCopy);
                    break;
                case 3:
                    blackboard.strings.Find(keybind) = blackboard.strings.Find(keybindCopy);
                    break;
                case 4:
                    blackboard.vector2s.Find(keybind) = blackboard.vector2s.Find(keybindCopy);
                    break;
                case 5:
                    blackboard.gameObjects.Find(keybind) = blackboard.gameObjects.Find(keybindCopy);
                    break;
                case 6:
                    blackboard.delegates.Find(keybind) = blackboard.delegates.Find(keybindCopy);
                    break;
            }
            return State.SUCCESS;
        }
        else
        {
            switch (variableIndex)
            {
                case 0:
                    blackboard.integers.Find(keybind) = intValue;
                    break;
                case 1:
                    blackboard.floats.Find(keybind) = floatValue;
                    break;
                case 2:
                    if (enableInvert)
                        blackboard.booleans.Find(keybind) = !blackboard.booleans.Find(keybind);
                    else
                        blackboard.booleans.Find(keybind) = boolValue;
                    break;
                case 3:
                    blackboard.strings.Find(keybind) = stringValue;
                    break;
                case 4:
                    blackboard.vector2s.Find(keybind) = vector2Value;
                    break;
                case 5:
                    blackboard.gameObjects.Find(keybind) = gameObjectValue;
                    break;
                case 6:
                    return State.FAILURE;
            }
            return State.SUCCESS;
        }
    }
    public override void OnBeforeSerialize()
    {
        if (prevVariableIndex != variableIndex)
        {
            prevVariableIndex = variableIndex;
            index = 0;
            indexCopy = 0;
            boolIndex = 0;
        }

        keybinds.Clear();
        keybindsCopy.Clear();

        switch (variableIndex)
        {
            case 0:
                foreach (Key<int> key in blackboard.integers.keys)
                    keybinds.Add(key.name + " (Integer Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    keybindsCopy.Add(key.name + " (Integer Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 1:
                foreach (Key<float> key in blackboard.floats.keys)
                    keybinds.Add(key.name + " (Float Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    keybindsCopy.Add(key.name + " (Float Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 2:
                foreach (Key<bool> key in blackboard.booleans.keys)
                    keybinds.Add(key.name + " (Boolean Key)");
                foreach (Key<bool> key in blackboard.booleans.keys)
                    keybindsCopy.Add(key.name + " (Boolean Key)");
                foreach (Key<bool> key in blackboard.booleans.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<bool> key in blackboard.booleans.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 3:
                foreach (Key<string> key in blackboard.strings.keys)
                    keybinds.Add(key.name + " (String Key)");
                foreach (Key<string> key in blackboard.strings.keys)
                    keybindsCopy.Add(key.name + " (String Key)");
                foreach (Key<string> key in blackboard.strings.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<string> key in blackboard.strings.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 4:
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    keybinds.Add(key.name + " (Vector2 Key)");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    keybindsCopy.Add(key.name + " (Vector2 Key)");
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<Vector2> key in blackboard.vector2s.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 5:
                foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                    keybinds.Add(key.name + " (GameObject Key)");
                foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                    keybindsCopy.Add(key.name + " (GameObject Key)");
                foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 6:
                foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
                    keybinds.Add(key.name + " (Delegate Key)");
                foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
                    keybindsCopy.Add(key.name + " (Delegate Key)");
                foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
        }
    }
}
