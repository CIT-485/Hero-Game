using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsMovement : MonoBehaviour
{  
    public float speed = .0001f;
    private float originalSpeed;
    private bool stop;
    private bool start;
    private Vector3 up;
    private void Start()
    {
        up = transform.position + Vector3.up * 2;
        originalSpeed = speed;
        StartCoroutine(MoveUp());
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            if (start)
            {
                Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0.75f, 0);
                Vector3 dirToPlayer = transform.position - target;
                Vector3 newPos = transform.position - (dirToPlayer * 1.25f * speed);
                transform.position = newPos;
            }
            else
            {
                Vector3 dirToPlayer = transform.position - up;
                Vector3 newPos = transform.position - (dirToPlayer * speed);
                transform.position = newPos;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stop = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    IEnumerator MoveUp()
    {
        yield return new WaitForSeconds(1);
        speed = originalSpeed * 0.75f;
        start = true;
    }
}
