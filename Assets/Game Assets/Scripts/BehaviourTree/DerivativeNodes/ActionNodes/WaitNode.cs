using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitNode : ActionNode
{
    public float duration = 1f;
    public bool enableKey = false;
    float startTime;
    protected override void OnStart() 
    {
        startTime = Time.time;

        if (enableKey)
        {
            if (blackboard.floats.Exist(keybind))
                duration = blackboard.floats.Find(keybind);
            else if (blackboard.integers.Exist(keybind))
                duration = blackboard.integers.Find(keybind);
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

        foreach (Key<int> key in blackboard.integers.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                keybind = key.name;
        foreach (Key<float> key in blackboard.floats.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                keybind = key.name;
    }
}
