using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessNode : DecoratorNode
{
    protected override State OnUpdate()
    {
        child.Update();
        if (child.state == State.RUNNING)
            return State.RUNNING;
        else
            return State.SUCCESS;
    }
}
