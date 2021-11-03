using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakNode : ActionNode
{
    protected override State OnUpdate() { return State.FAILURE; }
}
