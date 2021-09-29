using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSensor : MonoBehaviour
{
    public GameObject player;
    List<Vector3> points = new List<Vector3>();
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        points.Clear();
        
        RaycastHit2D[] hitR = Physics2D.RaycastAll(player.transform.position + new Vector3(0.375f, 1f), Vector2.down);
        RaycastHit2D[] hitL = Physics2D.RaycastAll(player.transform.position + new Vector3(-0.375f, 1f), Vector2.down);

        foreach (RaycastHit2D h in hitR)
            if (h && h.collider.tag == "Platform")
                if (!player.GetComponent<PlayerMovement>().grounded)
                    points.Add(h.point);

        foreach (RaycastHit2D h in hitL)
            if (h && h.collider.tag == "Platform")
                if (!player.GetComponent<PlayerMovement>().grounded)
                    points.Add(h.point);

        if (points.Count == 0)
        {
            //fail safe just in case
            transform.position = player.transform.position;
        }
        else
        {
            Vector3 highest = points[0];
            foreach (Vector3 point in points)
                if (point.y > highest.y)
                    highest = point;
            transform.position = new Vector2(player.transform.position.x, highest.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col == player.GetComponent<PlayerMovement>().hurtbox.GetComponent<Collider2D>())
        {
            player.GetComponent<PlayerMovement>().grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col == player.GetComponent<PlayerMovement>().hurtbox.GetComponent<Collider2D>())
        {
            player.GetComponent<PlayerMovement>().grounded = false;
        }
    }
}
