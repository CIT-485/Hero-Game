using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArithmeticNode : ActionNode
{
    public float floatValue;

    public int variableIndex = 0;
    public int prevVariableIndex = 0;
    public List<string> variableTypes = new List<string>() { "INTEGER", "FLOAT" };

    public int operatorIndex = 0;
    public List<string> operatorList = new List<string>();

    public string result;
    public int resultIndex = 0;
    public List<string> resultList = new List<string>();

    public List<string> keybindsCopy = new List<string>();
    public int indexCopy;
    public string keybindCopy;

    public bool enableKey = false;

    bool success;

    protected override State OnUpdate()
    {
        switch (variableIndex)
        {
            case 0:
                if (enableKey)
                {
                    if (blackboard.integers.Exist(keybindCopy))
                    {
                        switch (operatorIndex)
                        {
                            case 0:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) += blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) + blackboard.integers.Find(keybindCopy);
                                break;
                            case 1:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) -= blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) + blackboard.integers.Find(keybindCopy);
                                break;
                            case 2:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) *= blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) * blackboard.integers.Find(keybindCopy);
                                break;
                            case 3:
                                if (blackboard.integers.Find(keybindCopy) != 0)
                                {
                                    if (result == "*Operant 1*")
                                        blackboard.integers.Find(keybind) /= blackboard.integers.Find(keybindCopy);
                                    else
                                        blackboard.integers.Find(result) = blackboard.integers.Find(keybind) / blackboard.integers.Find(keybindCopy);
                                }
                                else
                                    return State.FAILURE;
                                break;
                        }
                    }
                    else
                    {
                        switch (operatorIndex)
                        {
                            case 0:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) += (int)blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) + (int)blackboard.floats.Find(keybindCopy);
                                break;
                            case 1:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) -= (int)blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) - (int)blackboard.floats.Find(keybindCopy);
                                break;
                            case 2:
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) *= (int)blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) * (int)blackboard.floats.Find(keybindCopy);
                                break;
                            case 3:
                                if ((int)blackboard.floats.Find(keybindCopy) != 0)
                                {
                                    if (result == "*Operant 1*")
                                        blackboard.integers.Find(keybind) /= (int)blackboard.floats.Find(keybindCopy);
                                    else
                                        blackboard.integers.Find(result) = blackboard.integers.Find(keybind) / (int)blackboard.floats.Find(keybindCopy);
                                }
                                else
                                    return State.FAILURE;
                                break;
                        }
                    }
                }
                else
                {
                    switch (operatorIndex)
                    {
                        case 0:
                            if (result == "*Operant 1*")
                                blackboard.integers.Find(keybind) += (int)floatValue;
                            else
                                blackboard.integers.Find(result) = blackboard.integers.Find(keybind) + (int)floatValue;
                            break;
                        case 1:
                            if (result == "*Operant 1*")
                                blackboard.integers.Find(keybind) -= (int)floatValue;
                            else
                                blackboard.integers.Find(result) = blackboard.integers.Find(keybind) - (int)floatValue;
                            break;
                        case 2:
                            if (result == "*Operant 1*")
                                blackboard.integers.Find(keybind) *= (int)floatValue;
                            else
                                blackboard.integers.Find(result) = blackboard.integers.Find(keybind) * (int)floatValue;
                            break;
                        case 3:
                            if ((int)floatValue != 0)
                            {
                                if (result == "*Operant 1*")
                                    blackboard.integers.Find(keybind) /= (int)floatValue;
                                else
                                    blackboard.integers.Find(result) = blackboard.integers.Find(keybind) / (int)floatValue;
                            }
                            else
                                return State.FAILURE;
                            break;
                    }
                }
                break;
            case 1:
                if (enableKey)
                {
                    if (blackboard.integers.Exist(keybindCopy))
                    {
                        switch (operatorIndex)
                        {
                            case 0:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) += blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) + blackboard.integers.Find(keybindCopy);
                                break;
                            case 1:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) -= blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) - blackboard.integers.Find(keybindCopy);
                                break;
                            case 2:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) *= blackboard.integers.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) * blackboard.integers.Find(keybindCopy);
                                break;
                            case 3:
                                if (blackboard.integers.Find(keybindCopy) != 0)
                                {
                                    if (result == "*Operant 1*")
                                        blackboard.floats.Find(keybind) /= blackboard.integers.Find(keybindCopy);
                                    else
                                        blackboard.floats.Find(result) = blackboard.floats.Find(keybind) / blackboard.integers.Find(keybindCopy);
                                }
                                else
                                    return State.FAILURE;
                                break;
                        }
                    }
                    else
                    {

                        switch (operatorIndex)
                        {
                            case 0:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) += blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) + blackboard.floats.Find(keybindCopy);
                                break;
                            case 1:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) -= blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) - blackboard.floats.Find(keybindCopy);
                                break;
                            case 2:
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) *= blackboard.floats.Find(keybindCopy);
                                else
                                    blackboard.floats.Find(result) = blackboard.floats.Find(keybind) * blackboard.floats.Find(keybindCopy);
                                break;
                            case 3:
                                if (blackboard.floats.Find(keybindCopy) != 0)
                                {
                                    if (result == "*Operant 1*")
                                        blackboard.floats.Find(keybind) /= blackboard.floats.Find(keybindCopy);
                                    else
                                        blackboard.floats.Find(result) = blackboard.floats.Find(keybind) / blackboard.floats.Find(keybindCopy);
                                }
                                else
                                    return State.FAILURE;
                                break;
                        }
                    }
                }
                else
                {
                    switch (operatorIndex)
                    {
                        case 0:
                            if (result == "*Operant 1*")
                                blackboard.floats.Find(keybind) += floatValue;
                            else
                                blackboard.floats.Find(result) = blackboard.integers.Find(keybind) + floatValue;
                            break;
                        case 1:
                            if (result == "*Operant 1*")
                                blackboard.floats.Find(keybind) -= floatValue;
                            else
                                blackboard.floats.Find(result) = blackboard.integers.Find(keybind) - floatValue;
                            break;
                        case 2:
                            if (result == "*Operant 1*")
                                blackboard.floats.Find(keybind) *= floatValue;
                            else
                                blackboard.floats.Find(result) = blackboard.integers.Find(keybind) * floatValue;
                            break;
                        case 3:
                            if ((int)floatValue != 0)
                            {
                                if (result == "*Operant 1*")
                                    blackboard.floats.Find(keybind) /= floatValue;
                                else
                                    blackboard.floats.Find(result) = blackboard.integers.Find(keybind) / floatValue;
                            }
                            else
                                return State.FAILURE;
                            break;
                    }
                }
                break;
        }
        return State.SUCCESS;
    }
    public override void OnBeforeSerialize()
    {
        if (prevVariableIndex != variableIndex)
        {
            prevVariableIndex = variableIndex;
            index = 0;
            indexCopy = 0;
            operatorIndex = 0;
            resultIndex = 0;
        }

        keybinds.Clear();
        keybindsCopy.Clear();
        resultList.Clear();

        resultList.Add("*Operand 1*");
        switch (variableIndex)
        {
            case 0:
                foreach (Key<int> key in blackboard.integers.keys)
                    keybinds.Add(key.name + " (Integer Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    resultList.Add(key.name + " (Integer Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    keybindsCopy.Add(key.name + " (Integer Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    keybindsCopy.Add(key.name + " (Float Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == resultList[resultIndex].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        result = key.name;
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
                foreach (Key<float> key in blackboard.floats.keys)
                    resultList.Add(key.name + " (Float Key)");
                foreach (Key<int> key in blackboard.integers.keys)
                    keybindsCopy.Add(key.name + " (Integer Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    keybindsCopy.Add(key.name + " (Float Key)");
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybind = key.name;
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == resultList[resultIndex].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        result = key.name;
                foreach (Key<int> key in blackboard.integers.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                foreach (Key<float> key in blackboard.floats.keys)
                    if (key.name == keybindsCopy[indexCopy].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                        keybindCopy = key.name;
                break;
        }
    }
}
