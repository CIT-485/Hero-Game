using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            if (col.transform.position.x > player.transform.position.x)
            {
                player.transform.position += new Vector3(-0.1f, -0.05f);
                col.transform.position += new Vector3(0.01f, 0);
            }
            if (col.transform.position.x < player.transform.position.x)
            {
                player.transform.position += new Vector3(0.1f, -0.05f);
                col.transform.position += new Vector3(-0.01f, 0);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Platform")
        {
            player.GetComponent<PlayerMovement>().grounded = false;
        }
    }
}
