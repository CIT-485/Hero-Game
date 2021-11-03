using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelSelectorNode : CompositeNode
{
    int current;
    protected override void OnStart() { current = 0; }
    protected override State OnUpdate()
    {
        State currentState = State.RUNNING;
        while (currentState == State.RUNNING)
        {
            var child = children[current];
            switch (child.Update())
            {
                case State.RUNNING:
                    break;
                case State.FAILURE:
                    current++;
                    break;
                case State.SUCCESS:
                    currentState = State.SUCCESS;
                    break;
            }
            if (current == children.Count)
                currentState = State.FAILURE;
        }
        return currentState;
    }
}
