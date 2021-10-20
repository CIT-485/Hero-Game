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
        // This is only needed if the enemies will have a collision box
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
        // This will flag the player's grounded state to false when the ground sensor leaves a platform
        if (col.tag == "Platform")
            player.GetComponent<Player>().grounded = false;
    }
}
