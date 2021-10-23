using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : DecoratorNode
{
    bool success = false;
    protected override void OnStart()
    {
        if (blackboard.booleans.Exist(keybind))
            success = blackboard.booleans.Find(keybind);
        else
        {
            success = false;
            Debug.LogWarning("WARNING: The key " + keybind + "does not currently exist! automatically set boolean to fail safe value: false");
        }
    }
    protected override State OnUpdate()
    {
        if (child != null)
        {
            if (success)
                child.Update();
            return success ? child.state : State.FAILURE;
        }
        return success ? State.SUCCESS : State.FAILURE;
    }
}
