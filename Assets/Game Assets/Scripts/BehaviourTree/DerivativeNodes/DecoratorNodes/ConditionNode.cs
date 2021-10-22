using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : DecoratorNode
{
    bool success = false;
    protected override void OnStart()
    {
        success = blackboard.booleans.Find(keybind);
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
