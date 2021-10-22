using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertNode : DecoratorNode
{
    protected override State OnUpdate()
    {
        child.Update();
        return child.state == State.SUCCESS ? State.FAILURE : child.state == State.FAILURE ? State.SUCCESS : State.RUNNING;
    }
}
