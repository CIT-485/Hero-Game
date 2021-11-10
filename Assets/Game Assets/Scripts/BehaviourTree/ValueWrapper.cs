using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ValueWrapper<T> where T : struct
{
    [SerializeField] private T value;
    public T Value { get {return value; } set { this.value = value; } }
    public ValueWrapper(T value) { this.Value = value; }
}
