using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToObject : MonoBehaviour
{
    public Transform obj;
    public Vector2 positionOffset;
    private void Update()
    {
        Move();
    }
    public void Move()
    {
        transform.position = obj.transform.position + new Vector3(positionOffset.x, positionOffset.y);
    }
}
