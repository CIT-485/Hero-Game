using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    protected override void OnStart()
    { Debug.Log("On Start: " + message); }
    protected override void OnStop()
    { Debug.Log("On Stop: " + message); }
    protected override State OnUpdate()
    {
        Debug.Log("On Update: " + message);
        return State.SUCCESS;
    }
}
