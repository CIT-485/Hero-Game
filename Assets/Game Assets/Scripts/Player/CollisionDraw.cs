using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDraw : MonoBehaviour
{
    public Collider2D box;
    public GameObject draw;
    void Update()
    {
        draw.SetActive(box.enabled);
    }
}
