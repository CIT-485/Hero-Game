using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomNumeralNode : ActionNode
{
    public int variableIndex;
    public List<string> variableTypes = new List<string>() { "INTEGER", "FLOAT" };

    public float lowerBound, upperBound;
    protected override State OnUpdate()
    {
        if (variableIndex == 0)
            blackboard.integers.Find(keybind) = Random.Range((int)lowerBound, (int)upperBound);
        else
            blackboard.floats.Find(keybind) = Random.Range(lowerBound, upperBound);
        return State.SUCCESS;
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        if (variableIndex == 0)
        {
            foreach (Key<int> key in blackboard.integers.keys)
                keybinds.Add(key.name + " (integer Key)");

            foreach (Key<int> key in blackboard.integers.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = key.name;
        }
        else
        {
            foreach (Key<float> key in blackboard.floats.keys)
                keybinds.Add(key.name + " (Float Key)");

            foreach (Key<float> key in blackboard.floats.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = key.name;
        }
    }
}
