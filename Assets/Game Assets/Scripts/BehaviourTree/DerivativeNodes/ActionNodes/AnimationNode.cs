using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNode : ActionNode
{
    public float floatValue;
    public int intValue;

    public int varIndex;
    public int prevCount;

    public int boolIndex;
    public List<string> boolList = new List<string>() { "True", "False" };
    public List<string> variables = new List<string>() { "Float", "Int", "Bool", "Trigger" };

    public string varName;

    public string[] chosenkeybind;
    protected override State OnUpdate()
    {
        if (blackboard.gameObjects.GetValue(chosenkeybind[0]).GetComponent<Animator>())
        {
            Animator animator = blackboard.gameObjects.GetValue(chosenkeybind[0]).GetComponent<Animator>();
            if (varIndex == 0)
                animator.SetFloat(varName, floatValue);
            else if (varIndex == 1)
                animator.SetInteger(varName, intValue);
            else if (varIndex == 2)
            {
                bool boolean = false;
                if (boolIndex == 0)
                    boolean = true;
                animator.SetBool(varName, boolean);
            }
            else
                animator.SetTrigger(varName);
            return State.SUCCESS;
        }
        return State.FAILURE;
    }
    public override void OnBeforeSerialize()
    {
        keybinds.Clear();

        foreach (Key<GameObject> key in blackboard.gameObjects.keys)
            keybinds.Add(key.name + " (GameObject Key)");

        chosenkeybind = new string[] { "", "" };

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
            chosenkeybind = keybinds[index].Split(new string[] { " (" }, System.StringSplitOptions.None);
            keybind = keybinds[index];
        }
    }
}
