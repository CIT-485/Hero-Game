using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    public bool enableKey = false;
    protected override void OnStart()
    {
        if (blackboard.integers.Exist(keybind))
            message = blackboard.integers.Find(keybind).ToString();
        if (blackboard.floats.Exist(keybind))
            message = blackboard.floats.Find(keybind).ToString();
        if (blackboard.strings.Exist(keybind))
            message = blackboard.strings.Find(keybind);
        if (blackboard.booleans.Exist(keybind))
            message = blackboard.booleans.Find(keybind).ToString();
        if (blackboard.delegates.Exist(keybind))
            message = blackboard.delegates.Find(keybind).ToString();
        if (blackboard.vector2s.Exist(keybind))
            message = blackboard.vector2s.Find(keybind).ToString();
        if (blackboard.gameObjects.Exist(keybind))
            message = blackboard.gameObjects.Find(keybind).ToString();
    }
    protected override State OnUpdate()
    {
        Debug.Log(message);
        return State.SUCCESS;
    }
}
