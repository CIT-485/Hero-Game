using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraStart : MonoBehaviour
{
    public float speed = 1f;
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
    }
}
