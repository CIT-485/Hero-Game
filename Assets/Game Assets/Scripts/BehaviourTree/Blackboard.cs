using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VariableType { INTEGER, FLOAT, BOOLEAN, STRING, VECTOR2, GAMEOBJECT, DELEGATE }

[System.Serializable]
public class Blackboard
{
    public Group<int> integers = new Group<int>();
    public Group<float> floats = new Group<float>();
    public Group<string> strings = new Group<string>();
    public Group<bool> booleans = new Group<bool>();
    public Group<Vector2> vector2s = new Group<Vector2>();
    public Group<GameObject> gameObjects = new Group<GameObject>();
    public Group<DoSomething> delegates = new Group<DoSomething>();

    public delegate Node.State DoSomething();
}

[System.Serializable]
public class Group<T>
{
    public List<Key<T>> keys = new List<Key<T>>();
    private T dummy = default(T);
    private Key<T> dummmy = new Key<T>();

    public ref T GetValue(string name)
    {
        foreach (Key<T> key in keys)
            if (key.name == name)
                return ref key.value;
        return ref dummy;
    }
    public Key<T> GetKey(string name)
    {
        foreach (Key<T> key in keys)
            if (key.name == name)
                return key;
        return default;
    }
    public void Add(string name, T value)
    {
        keys.Add(new Key<T>(name, value));
    }
    public void Remove(string name)
    {
        foreach (Key<T> key in keys)
            if (key.name == name)
                keys.Remove(key);
    }
    public bool Exist(string name)
    {
        List<string> names = new List<string>();
        foreach (Key<T> key in keys)
            names.Add(key.name);
        if (names.Contains(name))
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class Key<T>
{
    public string name;
    public T value;
    public Type type;
    public Key(string name, T value)
    {
        this.name = name;
        this.value = value;
        type = value.GetType();
    }
    public Key() { }
}