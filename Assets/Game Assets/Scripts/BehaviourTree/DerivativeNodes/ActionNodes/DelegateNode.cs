using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateNode : ActionNode
{
    protected override State OnUpdate()
    {
        return blackboard.delegates.Find(keybind)();
    }
}
