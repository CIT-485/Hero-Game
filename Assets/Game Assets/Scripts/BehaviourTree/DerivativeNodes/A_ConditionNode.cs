using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ConditionNode : ActionNode
{
    bool success = false;
    protected override void OnStart()
    {
        doneOnce = true;
        foreach (Key<bool> key in blackboard.boolKeys)
            if (keybind == key.name)
                success = key.value;
    }
    protected override State OnUpdate()
    {
        return success ? State.SUCCESS : State.FAILURE;
    }
}