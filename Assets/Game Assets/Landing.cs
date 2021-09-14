using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landing : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(player.transform.position + Vector3.up, Vector2.down);
        foreach (RaycastHit2D h in hit)
        {
            if (h && h.collider.tag == "Platform")
            {
                if (!player.GetComponent<PlayerMovement>().grounded)
                    transform.position = h.point;
                else
                    transform.position = player.transform.position;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            player.GetComponent<PlayerMovement>().grounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == player)
        {
            player.GetComponent<PlayerMovement>().grounded = false;
        }
    }
}
