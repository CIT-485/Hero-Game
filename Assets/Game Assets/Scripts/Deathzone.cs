using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deathzone : MonoBehaviour
{
    public GameObject empty;
    private GameObject player;
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowObject>().objectToFollow = Instantiate(empty, player.transform.position, player.transform.rotation).transform;
            player.GetComponent<Player>().healthBar.TakeDamage(player.GetComponent<Player>().healthBar.maxHealth * 5);
        }
    }
}
