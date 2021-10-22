using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : ActionNode
{
    bool success;
    protected override void OnStart()
    {
        foreach (Key<bool> key in blackboard.boolKeys)
            if (keybind == key.name)
                success = key.value;
    }
    protected override void OnStop() { }
    protected override State OnUpdate()
    {
        return success ? State.SUCCESS : State.FAILURE;
    }
}