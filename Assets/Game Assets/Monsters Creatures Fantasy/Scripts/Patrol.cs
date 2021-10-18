using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    // How fast the enemy moves 
    public float speed;

    //
    private float waitTime;
    public float startWaitTime;

    // All of the positions the enemy can potentially move to
    public Transform[] moveSpots;

    // A random position from the moveSpots array
    private int randomSpot;


    // Start is called before the first frame update
    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy moves to this random spot
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        
        // Checks if the enemy has made it to the random position 
        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            // How long an enemy waits before moving to a new location
            if(waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            } else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
