using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowObject>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthBar.TakeDamage(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthBar.maxHealth * 5);
        }
    }
}
