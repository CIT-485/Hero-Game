using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State { RUNNING, FAILURE, SUCCESS }
    public State state = State.RUNNING;
    public bool started = false;
    public bool doneOnce = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Blackboard blackboard;
    [TextArea] public string description;
    public string keybind;
    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }
        state = OnUpdate();
        if (state == State.FAILURE || state == State.SUCCESS)
        {
            OnStop();
            started = false;
        }
        return state;
    }
    public virtual Node Clone()
    {
        return Instantiate(this);
    }
    protected virtual void OnStart() { }
    protected virtual void OnStop() { doneOnce = true; }
    protected abstract State OnUpdate();

}
