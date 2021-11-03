using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGNode : ActionNode
{
    public float lowerBound;
    public float upperBound;

    public int prevCount;

    public string[] chosenKeybind;

    protected override void OnStart()
    {
        chosenKeybind = keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None);

        float randomNum = Random.Range(lowerBound, upperBound);
        
        if (chosenKeybind[1].Contains("Integer"))
            blackboard.integers.GetValue(chosenKeybind[0]) = (int)randomNum;
        else if (chosenKeybind[1].Contains("Float"))
            blackboard.floats.GetValue(chosenKeybind[0]) = randomNum;
        else if (chosenKeybind[1].Contains("[X]"))
            blackboard.vector2s.GetValue(chosenKeybind[0]).x = randomNum;
        else
            blackboard.vector2s.GetValue(chosenKeybind[0]).y = randomNum;
    }
    protected override State OnUpdate()
    {
        return State.SUCCESS;
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();

        foreach (Key<int> key in blackboard.integers.keys)
            keybinds.Add(key.name + " (Integer Key)");
        foreach (Key<float> key in blackboard.floats.keys)
            keybinds.Add(key.name + " (Float Key)");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key [X])");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key [Y])");

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
        }

        if (keybinds.Count > 0)
        {
            chosenKeybind = keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None);
            keybind = keybinds[index];
        }
    }
}