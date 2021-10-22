using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    string message;
    protected override void OnStart()
    {
        foreach (Key<string> key in blackboard.stringKeys)
            if (keybind == key.name)
                message = key.value;
    }
    protected override State OnUpdate()
    {
        Debug.Log(message);
        return State.SUCCESS;
    }
}
