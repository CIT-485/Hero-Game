using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    // How fast an enemy run's after the player
    public float speed;

    // The distance an enemy should stop when reaching the player
    public float stoppingDistance;

    // The distance an enemy should back away from the player
    public float retreatDistance;

    // Variable that holds which Game object that the enemy is chasing after
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks the distance between the enemy and the player
        // If the enemy isn't close to the player then contiue moving
        // If the enemy is close to the player then chase after the player
        if(Vector2.Distance(transform.position, target.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
    }
}
