using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    [HideInInspector] public bool loopInfinitely = true;
    [HideInInspector] public int loopCount = 0;
    [HideInInspector] public int count;
    [HideInInspector] public int prevCountForState;
    [HideInInspector] public bool enableKeyloop = false;
    public string keybindName;
    public int prevCount;

    protected override void OnStart() 
    { 
        count = 1;
        prevCountForState = 0;
        if (!loopInfinitely)
        {
            if (blackboard.integers.Exist(keybind) && enableKeyloop)
                loopCount = blackboard.integers.GetValue(keybind);
            else if (enableKeyloop)
            {
                loopCount = 0;
                Debug.LogWarning("WARNING: The key \"" + keybind + "\" does not currently exist! automatically set loopCount to fail safe value: 1");
            }
        }
    }
    protected override State OnUpdate()
    {
        child.Update();
        if (child.state != State.RUNNING)
        {
            if (count < loopCount || loopInfinitely)
            {
                count++;
                Traverse(this, node =>
                {
                    if (node != this)
                    {
                        node.doneOnce = false;
                        node.started = false;
                        node.state = State.RUNNING;
                    }
                });
            }
            else
            {
                return State.SUCCESS;
            }
        }
        return State.RUNNING;
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.child != null)
        {
            children.Add(decorator.child);
        }

        RootNode root = parent as RootNode;
        if (root && root.child != null)
        {
            children.Add(root.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.children;
        }
        return children;
    }

    public void Traverse(Node node, System.Action<Node> visitor)
    {
        if (node)
        {
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visitor));
        }
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();
        foreach (Key<int> key in blackboard.integers.keys)
            keybinds.Add(key.name + " (integer Key)");

        if (prevCount != keybinds.Count)
        {
            prevCount = keybinds.Count;
            bool found = false;
            int i = 0;
            for (i = 0; i < keybinds.Count && !found; i++)
                if (keybinds[i] == keybindName)
                    found = true;
            if (found)
                index = i - 1;
            else
                index = 0;
        }

        foreach (Key<int> key in blackboard.integers.keys)
            if (key.name == keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None)[0])
            {
                keybind = key.name;
                keybindName = keybinds[index];
            }
    }
}