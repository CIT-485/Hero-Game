using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    // Detection Point
    public Transform detectionPoint;
    // Detection radius
    private const float detectionRadius = 0.2f;
    // Detection layer
    public LayerMask detectionLayer;
    // Update is called once per frame
    void Update()
    {
        if(DetectObject())
        {
            if(InteractInput())
            {
                Debug.Log("INTERACTION");
            }
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    // Detect whether your interacting with an object
    bool DetectObject()
    {
        return Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);
    }
}
