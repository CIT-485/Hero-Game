using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraStart : MonoBehaviour
{
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0);
    }
}
