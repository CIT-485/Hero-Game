using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDelegateScript : MonoBehaviour
{
    BehaviourTreeController btc;
    public int a = 0;
    void Start()
    {
        btc = GetComponent<BehaviourTreeController>();
        btc.tree.blackboard.delegates.GetValue("AAA") = DelegateTest;
    }

    void Update()
    {
        btc.tree.blackboard.vector2s.GetValue("Velocity") = GetComponent<Rigidbody2D>().velocity;
    }

    public Node.State DelegateTest()
    {
        a++;
        Debug.Log(a);
        if (a < 100)
        {
            Debug.Log("RUNNIGN");
            return Node.State.RUNNING;
        }
        else
        {
            Debug.Log("SUCCESS");
            return Node.State.SUCCESS;
        }
    }
}
