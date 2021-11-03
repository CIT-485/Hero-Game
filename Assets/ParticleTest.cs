using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{

    public KeyCode move;
    public GameObject target;
    public float speed = .0001f;
    public ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("ParticleTarget");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dirToPlayer = transform.position - target.transform.position;
        Vector3 newPos = transform.position - (dirToPlayer.normalized * speed);
        transform.position = newPos;
    }

    public void Fade()
    {
        StartCoroutine("timing");
    }

    IEnumerator timing()
    {
        particles.Stop(true);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

}
