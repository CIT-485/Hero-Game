using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1f;
    public bool enableKey = false;
    public int prevCount;
    public string keybindName;
    float startTime;
    protected override void OnStart() 
    {
        startTime = Time.time;

        if (enableKey)
        {
            if (blackboard.floats.Exist(keybind))
                duration = blackboard.floats.GetValue(keybind);
            else if (blackboard.integers.Exist(keybind))
                duration = blackboard.integers.GetValue(keybind);
        }
    }
    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
            return State.SUCCESS;
        return State.RUNNING;
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        foreach (Key<int> key in blackboard.integers.keys)
            keybinds.Add(key.name + " (Integer Key)");
        foreach (Key<float> key in blackboard.floats.keys)
            keybinds.Add(key.name + " (Float Key)");

        if (prevCount != keybinds.Count)
        {
            prevCount = keybinds.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < keybinds.Count && !found; i++)
                if (keybinds[i] == keybindName)
                    found = true;
            if (found)
                index = i - 1;
            else
                index = 0;
        }

        foreach (Key<int> key in blackboard.integers.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
            {
                keybind = key.name;
                keybindName = keybinds[index];
            }
        foreach (Key<float> key in blackboard.floats.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
            {
                keybind = key.name;
                keybindName = keybinds[index];
            }
    }
}
