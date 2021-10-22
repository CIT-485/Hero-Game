using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallelNode : CompositeNode
{
    List<Node.State> list = new List<Node.State>();
    protected override void OnStart() 
    {
        list.Clear();
        foreach (Node child in children)
        {
            list.Add(State.RUNNING);
        }
    }
    protected override State OnUpdate()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].state == State.RUNNING)
                children[i].Update();
            if (children[i].state == State.SUCCESS || children[i].state == State.FAILURE)
                list[i] = children[i].state;
        }
        if (list.All((c) => c == State.SUCCESS))
            return State.SUCCESS;
        else if (list.Any((c) => c == State.FAILURE))
            return State.FAILURE;
        else
            return State.RUNNING;
    }
}
