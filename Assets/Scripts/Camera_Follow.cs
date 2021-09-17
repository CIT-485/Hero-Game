using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //stores current camera position in temp
        Vector3 temp = transform.position;

        //set cameras position to player's position x
        temp.x = playerTransform.position.x;

        temp.y = playerTransform.position.y + 2.5f;

        transform.position = temp;
    }
}
