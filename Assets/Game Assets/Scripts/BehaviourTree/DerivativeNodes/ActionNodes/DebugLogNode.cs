using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string message;
    public int prevCount;
    public bool enableKey = false;
    protected override void OnStart()
    {
        if (enableKey)
        {
            if (blackboard.integers.Exist(keybind))
                message = blackboard.integers.GetValue(keybind).ToString();
            if (blackboard.floats.Exist(keybind))
                message = blackboard.floats.GetValue(keybind).ToString();
            if (blackboard.strings.Exist(keybind))
                message = blackboard.strings.GetValue(keybind);
            if (blackboard.booleans.Exist(keybind))
                message = blackboard.booleans.GetValue(keybind).ToString();
            if (blackboard.vector2s.Exist(keybind))
                message = blackboard.vector2s.GetValue(keybind).ToString();
            if (blackboard.gameObjects.Exist(keybind))
                message = blackboard.gameObjects.GetValue(keybind).ToString();
            if (blackboard.delegates.Exist(keybind))
                message = blackboard.delegates.GetValue(keybind).ToString();
        }
    }
    protected override State OnUpdate()
    {
        Debug.Log(message);
        return State.SUCCESS;
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        foreach (Key<int> key in blackboard.integers.keys)
            keybinds.Add(key.name + " (Integer Key)");
        foreach (Key<float> key in blackboard.floats.keys)
            keybinds.Add(key.name + " (Float Key)");
        foreach (Key<bool> key in blackboard.booleans.keys)
            keybinds.Add(key.name + " (Boolean Key)");
        foreach (Key<string> key in blackboard.strings.keys)
            keybinds.Add(key.name + " (string Key)");
        foreach (Key<Vector2> key in blackboard.vector2s.keys)
            keybinds.Add(key.name + " (Vector2 Key)");
        foreach (Key<GameObject> key in blackboard.gameObjects.keys)
            keybinds.Add(key.name + " (GameObject Key)");
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

        if (keybinds.Count > 0)
        {
            foreach (Key<int> key in blackboard.integers.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<float> key in blackboard.floats.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<bool> key in blackboard.booleans.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<string> key in blackboard.strings.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<Vector2> key in blackboard.vector2s.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<GameObject> key in blackboard.gameObjects.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
            foreach (Key<Blackboard.DoSomething> key in blackboard.delegates.keys)
                if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
                    keybind = keybinds[index];
        }
    }
}
