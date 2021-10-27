using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateNode : ActionNode
{
    protected override State OnUpdate()
    {
        return blackboard.delegates.Find(keybind)();
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
            keybinds.Add(key.name + " (Delegate Key)");

        foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                keybind = key.name;
    }
}
