using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : DecoratorNode
{
    public int intValue;
    public float floatValue;
    public bool boolValue;
    public string stringValue;
    public Vector2 vector2Value;
    public GameObject gameObjectValue;

    public int variableIndex = 0;
    public int prevVariableIndex = 0;

    public int operatorIndex = 0;
    public List<string> operatorList = new List<string>() { "=", "!=", "<", "<=", ">", ">=" };

    public List<string> keybindsCopy = new List<string>();
    public int indexCopy;
    public string keybindCopy;

    public bool enableKey = false;
    public bool enableInvert = false;
    public bool comparePercent = false;
    public int boolIndex;

    bool success;

    protected override void OnStart()
    {
        success = false;
        if (boolIndex == 0)
            boolValue = true;
        else
            boolValue = false;
    }
    protected override State OnUpdate()
    {
        if (!success)
        {
            if (enableKey)
            {
                switch (variableIndex)
                {
                    case 0:
                        if (comparePercent)
                        {
                            if (blackboard.integers.Exist(keybindCopy))
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.integers.Find(keybind) == blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.integers.Find(keybind) != blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.integers.Find(keybind) < blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.integers.Find(keybind) <= blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.integers.Find(keybind) > blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.integers.Find(keybind) >= blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                }
                            }
                            else
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.integers.Find(keybind) == blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.integers.Find(keybind) != blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.integers.Find(keybind) < blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.integers.Find(keybind) <= blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.integers.Find(keybind) > blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.integers.Find(keybind) >= blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (blackboard.integers.Exist(keybindCopy))
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.integers.Find(keybind) == blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.integers.Find(keybind) != blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.integers.Find(keybind) < blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.integers.Find(keybind) <= blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.integers.Find(keybind) > blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.integers.Find(keybind) >= blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                }
                            }
                            else
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.integers.Find(keybind) == blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.integers.Find(keybind) != blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.integers.Find(keybind) < blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.integers.Find(keybind) <= blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.integers.Find(keybind) > blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.integers.Find(keybind) >= blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                }
                            }
                        }
                        break;
                    case 1:
                        if (comparePercent)
                        {
                            if (blackboard.floats.Exist(keybindCopy))
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.floats.Find(keybind) == blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.floats.Find(keybind) != blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.floats.Find(keybind) < blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.floats.Find(keybind) <= blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.floats.Find(keybind) > blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.floats.Find(keybind) >= blackboard.floats.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                }
                            }
                            else
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.floats.Find(keybind) == blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.floats.Find(keybind) != blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.floats.Find(keybind) < blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.floats.Find(keybind) <= blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.floats.Find(keybind) > blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.floats.Find(keybind) >= blackboard.integers.Find(keybindCopy) * floatValue / 100)
                                            success = true;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (blackboard.floats.Exist(keybindCopy))
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.floats.Find(keybind) == blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.floats.Find(keybind) != blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.floats.Find(keybind) < blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.floats.Find(keybind) <= blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.floats.Find(keybind) > blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.floats.Find(keybind) >= blackboard.floats.Find(keybindCopy))
                                            success = true;
                                        break;
                                }
                            }
                            else
                            {
                                switch (operatorIndex)
                                {
                                    case 0:
                                        if (blackboard.floats.Find(keybind) == blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 1:
                                        if (blackboard.floats.Find(keybind) != blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 2:
                                        if (blackboard.floats.Find(keybind) < blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 3:
                                        if (blackboard.floats.Find(keybind) <= blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 4:
                                        if (blackboard.floats.Find(keybind) > blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                    case 5:
                                        if (blackboard.floats.Find(keybind) >= blackboard.integers.Find(keybindCopy))
                                            success = true;
                                        break;
                                }
                            }
                        }
                        break;
                    case 2:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.booleans.Find(keybind) == boolValue)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.booleans.Find(keybind) != boolValue)
                                    success = true;
                                break;
                        }
                        break;
                    case 3:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.strings.Find(keybind) == blackboard.strings.Find(keybindCopy))
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.strings.Find(keybind) != blackboard.strings.Find(keybindCopy))
                                    success = true;
                                break;
                            case 2:
                                if (blackboard.strings.Find(keybind).Contains(blackboard.strings.Find(keybindCopy)))
                                    success = true;
                                break;
                        }
                        break;
                    case 4:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.floats.Find(keybind) == blackboard.floats.Find(keybindCopy))
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.floats.Find(keybind) != blackboard.floats.Find(keybindCopy))
                                    success = true;
                                break;
                        }
                        break;
                    case 5:
                        return State.FAILURE;
                    case 6:
                        return State.FAILURE;
                }
            }
            else
            {
                switch (variableIndex)
                {
                    case 0:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.integers.Find(keybind) == floatValue)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.integers.Find(keybind) != floatValue)
                                    success = true;
                                break;
                            case 2:
                                if (blackboard.integers.Find(keybind) < floatValue)
                                    success = true;
                                break;
                            case 3:
                                if (blackboard.integers.Find(keybind) <= floatValue)
                                    success = true;
                                break;
                            case 4:
                                if (blackboard.integers.Find(keybind) > floatValue)
                                    success = true;
                                break;
                            case 5:
                                if (blackboard.integers.Find(keybind) >= floatValue)
                                    success = true;
                                break;
                        }
                        break;
                    case 1:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.floats.Find(keybind) == floatValue)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.floats.Find(keybind) != floatValue)
                                    success = true;
                                break;
                            case 2:
                                if (blackboard.floats.Find(keybind) < floatValue)
                                    success = true;
                                break;
                            case 3:
                                if (blackboard.floats.Find(keybind) <= floatValue)
                                    success = true;
                                break;
                            case 4:
                                if (blackboard.floats.Find(keybind) > floatValue)
                                    success = true;
                                break;
                            case 5:
                                if (blackboard.floats.Find(keybind) >= floatValue)
                                    success = true;
                                break;
                        }
                        break;
                    case 2:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.booleans.Find(keybind) == boolValue)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.booleans.Find(keybind) != boolValue)
                                    success = true;
                                break;
                        }
                        break;
                    case 3:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.strings.Find(keybind) == stringValue)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.strings.Find(keybind) != stringValue)
                                    success = true;
                                break;
                            case 2:
                                if (blackboard.strings.Find(keybind).Contains(stringValue))
                                    success = true;
                                break;
                        }
                        break;
                    case 4:
                        switch (operatorIndex)
                        {
                            case 0:
                                if (blackboard.vector2s.Find(keybind) == vector2Value)
                                    success = true;
                                break;
                            case 1:
                                if (blackboard.vector2s.Find(keybind) != vector2Value)
                                    success = true;
                                break;
                        }
                        break;
                    case 5:
                        return State.FAILURE;
                    case 6:
                        return State.FAILURE;
                }
            }
        }
        else
            child.Update();
        return success ? child.state : State.FAILURE;
    }
    public override void OnBeforeSerialize()
    {
        if (prevVariableIndex != variableIndex)
        {
            prevVariableIndex = variableIndex;
            index = 0;
            indexCopy = 0;
            boolIndex = 0;
            operatorIndex = 0;
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
                foreach (Key<float> key in blackboard.floats.keys)
                    keybindsCopy.Add(key.name + " (Float Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
            case 1:
                foreach (Key<float> key in blackboard.floats.keys)
                    keybinds.Add(key.name + " (Float Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    keybindsCopy.Add(key.name + " (Integer Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    keybindsCopy.Add(key.name + " (Float Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
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
