using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyHitbox")
        {
            player.GetComponent<PlayerCombat>().DeactivateHitboxes();
            player.GetComponent<PlayerCombat>().animator.SetTrigger("Hurt");
            //player.GetComponent<PlayerCombat>().healthBar.TakeDamage(collision.transform.parent.GetComponent<Damages>().activeDamage);
            if (collision.transform.parent.position.x < transform.position.x)
                player.GetComponent<PlayerCombat>().body2d.AddForce(new Vector2(75, 30));
            else
                player.GetComponent<PlayerCombat>().body2d.AddForce(new Vector2(-75, 30));
        }
    }
}
