using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateNode : ActionNode
{
    public int prevCount;
    protected override State OnUpdate()
    {
        return blackboard.delegates.GetValue(keybind.Split(new string[] { " (" }, System.StringSplitOptions.None)[0])();
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
            keybinds.Add(key.name + " (Delegate Key)");


        if (prevCount != keybinds.Count)
        {
            prevCount = keybinds.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < keybinds.Count && !found; i++)
                if (keybinds[i] == keybind)
                    found = true;
            if (found)
                index = i - 1;
            else
                index = 0;
        }

        foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                keybind = keybinds[index];
    }
}
