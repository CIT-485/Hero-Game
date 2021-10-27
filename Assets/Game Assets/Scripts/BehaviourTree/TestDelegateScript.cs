using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDelegateScript : MonoBehaviour
{
    BehaviourTreeController btc;
    void Start()
    {
        btc = GetComponent<BehaviourTreeController>();
        if (btc.tree.blackboard.delegates.Exist("DelegateTest"))
            btc.tree.blackboard.delegates.Find("DelegateTest") = DelegateTest;
        else
            btc.tree.blackboard.delegates.Add("DelegateTest", DelegateTest);
    }

    public Node.State DelegateTest()
    {
        if (!btc.tree.blackboard.floats.Exist("FloatDelegate"))
            btc.tree.blackboard.floats.Add("FloatDelegate", 0f);
        btc.tree.blackboard.floats.Find("FloatDelegate")++;
        return Node.State.SUCCESS;
    }
}
