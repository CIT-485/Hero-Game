using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject, ISerializationCallbackReceiver
{
    public enum State { RUNNING, FAILURE, SUCCESS }
    [HideInInspector] public State state = State.RUNNING;
    [HideInInspector] public int index;
    [HideInInspector] public List<string> keybinds = new List<string>();
    [HideInInspector] public bool started = false;
    [HideInInspector] public bool doneOnce = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Blackboard blackboard;
    [HideInInspector] public string description;
    [HideInInspector] public string keybind;
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
    public virtual void OnBeforeSerialize() { }
    public virtual void OnAfterDeserialize() { }
}
