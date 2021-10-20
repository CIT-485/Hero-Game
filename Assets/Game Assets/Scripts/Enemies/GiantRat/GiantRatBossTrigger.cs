using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantRatBossTrigger : MonoBehaviour
{
    private Camera      m_cam;
    public GameObject   giantRat;
    private GameObject  player;
    public GameObject   giantRatBoss;
    public GameObject   giantRatCinematic;
    void Start()
    {
        m_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == player.GetComponent<Player>().hurtbox.GetComponent<Collider2D>())
        {
            StartCoroutine(Cinematic());
        }
    }
    IEnumerator Cinematic()
    {
        CameraFollowObject followScript = m_cam.GetComponent<CameraFollowObject>();
        followScript.objectToFollow = giantRat.transform;
        followScript.positionOffset = new Vector2(0, 2);
        followScript.cameraSpeed = 1.5f;
        player.GetComponent<Player>().body2d.velocity = Vector3.zero;
        player.GetComponent<Player>().actionAllowed = false;
        yield return new WaitForSeconds(4);
        followScript.objectToFollow = player.transform;
        followScript.cameraSpeed = 3f;
        giantRatBoss.SetActive(true);
        giantRatCinematic.SetActive(false);
        Destroy(giantRat.GetComponent<Rigidbody2D>());
        player.GetComponent<Player>().actionAllowed = true;
        yield return new WaitForSeconds(1.2f);
        followScript.cameraSpeed = 5f;
        GameObject.Destroy(this.gameObject);
    }
}
