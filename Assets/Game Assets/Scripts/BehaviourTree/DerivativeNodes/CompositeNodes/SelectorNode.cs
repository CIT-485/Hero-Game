using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : CompositeNode
{
    int current;
    protected override void OnStart() { current = 0; }
    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.RUNNING:
                return State.RUNNING;
            case State.FAILURE:
                current++;
                break;
            case State.SUCCESS:
                return State.SUCCESS;
        }
        return current == children.Count ? State.FAILURE : State.RUNNING;
    }
}
