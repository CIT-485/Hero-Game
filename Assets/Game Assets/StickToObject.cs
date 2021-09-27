using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToObject : MonoBehaviour
{
    public Transform obj;
    public Vector2 positionOffset;
    private void Update()
    {
        transform.position = obj.transform.position + new Vector3(positionOffset.x, positionOffset.y);
    }
}
