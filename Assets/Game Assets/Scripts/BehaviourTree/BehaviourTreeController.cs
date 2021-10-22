using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeController : MonoBehaviour
{
    public BehaviourTree tree;
    // Start is called before the first frame update
    void Awake()
    {
        tree = tree.Clone();
        tree.Bind();
    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
