using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard
{
    [SerializeField]
    public List<Key<int>> intKeys = new List<Key<int>>();
    public List<Key<float>> floatKeys = new List<Key<float>>();
    public List<Key<string>> stringKeys = new List<Key<string>>();
    public List<Key<bool>> boolKeys = new List<Key<bool>>();
    public Dictionary<string, Key<int>> blackboard = new Dictionary<string, Key<int>>();
}

[System.Serializable]
public class Key<T>
{
    public string name;
    public T value;

    public Key(string name, T value)
    {
        this.name = name;
        this.value = value;
    }
    public Key() { }
}