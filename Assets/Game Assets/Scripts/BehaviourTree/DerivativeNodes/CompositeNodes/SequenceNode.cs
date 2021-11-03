using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : CompositeNode
{
    int current;
    protected override void OnStart() { current = 0; }
    protected override State OnUpdate()
    {
        while (current < children.Count)
        {
            var child = children[current];
            switch (child.Update())
            {
                case State.RUNNING:
                    return State.RUNNING;
                case State.FAILURE:
                    return State.FAILURE;
                case State.SUCCESS:
                    current++;
                    break;
            }
        }
        return State.SUCCESS;
    }
}
