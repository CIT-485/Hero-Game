using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSensor : MonoBehaviour
{
    public GameObject entity;
    List<Vector3> points = new List<Vector3>();
    void Update()
    {
        // We will collect a list of points that a raycast will come in contact with, then we will find the highest vertical point
        // in the list and then place the landing sensor on that point.
        points.Clear();
        
        RaycastHit2D[] hitR = Physics2D.RaycastAll(entity.transform.position + new Vector3(0.375f, 1f), Vector2.down);
        RaycastHit2D[] hitL = Physics2D.RaycastAll(entity.transform.position + new Vector3(-0.375f, 1f), Vector2.down);

        foreach (RaycastHit2D h in hitR)
            if (h && h.collider.tag == "Platform")
                if (!entity.GetComponent<IEntity>().Grounded)
                    points.Add(h.point);

        foreach (RaycastHit2D h in hitL)
            if (h && h.collider.tag == "Platform")
                if (!entity.GetComponent<IEntity>().Grounded)
                    points.Add(h.point);

        // If the raycast cannot find a platform underneath the player, then we will have a fail safe so things don't crash
        if (points.Count == 0)
            transform.position = entity.transform.position;
        else
        {
            Vector3 highest = points[0];
            foreach (Vector3 point in points)
                if (point.y > highest.y)
                    highest = point;
            transform.position = new Vector2(entity.transform.position.x, highest.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // If the player enter the landing sensor, then the player's grounded state will be flagged as true
        if (col == entity.GetComponent<Collider2D>())
            entity.GetComponent<IEntity>().Grounded = true;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        // If the player exits the landing sensor, then the player's grounded state will be flagged as false
        if (col == entity.GetComponent<Collider2D>())
            entity.GetComponent<IEntity>().Grounded = false;
    }
}
