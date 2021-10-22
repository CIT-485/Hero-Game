using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_ConditionNode : DecoratorNode
{
    bool success = false;
    protected override void OnStart()
    {
        foreach (Key<bool> key in blackboard.boolKeys)
            if (keybind == key.name)
                success = key.value;
    }
    protected override State OnUpdate()
    {
        if (success)
            child.Update();
        return success ? child.state : State.FAILURE;
    }
}
