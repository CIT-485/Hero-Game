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
        if (blackboard.floats.Exist(keybind) && enableKey)
            duration = blackboard.floats.Find(keybind);
        else if (blackboard.integers.Exist(keybind) && enableKey)
            duration = blackboard.integers.Find(keybind);
        else if (enableKey)
        {
            duration = 1f;
            Debug.LogWarning("WARNING: The key " + keybind + "does not currently exist! automatically set duration to fail safe value: 1f");
        }
    }
    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
            return State.SUCCESS;
        return State.RUNNING;
    }
}
