using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeController : MonoBehaviour
{
    public BehaviourTree tree;
    // Start is called before the first frame update
    void Awake()
    {
        BehaviourTree clone = tree.Clone();
        tree = clone;
        tree.Bind();
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
