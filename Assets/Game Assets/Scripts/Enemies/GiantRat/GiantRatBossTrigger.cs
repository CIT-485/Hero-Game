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
    public GameObject   bossMusic;
    public GameObject   bossHealthBar;
    bool turnOffMusic = false;
    void Start()
    {
        m_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (turnOffMusic)
        {
            if (GameObject.Find("BGM"))
            {
                GameObject.Find("BGM").GetComponent<AudioSource>().volume -= Time.deltaTime / 25;
                if (GameObject.Find("BGM").GetComponent<AudioSource>().volume < 0.001f)
                    Destroy(GameObject.Find("BGM"));
            }
        }
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
        GetComponent<Collider2D>().enabled = false;
        player.GetComponent<Player>().playerCanvas.GetComponent<Animator>().SetTrigger("Hide");
        turnOffMusic = true;
        CameraFollowObject followScript = m_cam.GetComponent<CameraFollowObject>();
        followScript.objectToFollow = giantRat.transform;
        followScript.positionOffset = new Vector2(0, 2);
        followScript.cameraSpeed = 1.5f;
        player.GetComponent<Player>().body2d.velocity = Vector3.zero;
        player.GetComponent<Player>().actionAllowed = false;
        player.GetComponent<Player>().aspectRatio.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(4);
        player.GetComponent<Player>().aspectRatio.GetComponent<Animator>().SetTrigger("FadeOut");
        player.GetComponent<Player>().playerCanvas.GetComponent<Animator>().SetTrigger("Show");
        followScript.objectToFollow = player.transform;
        followScript.cameraSpeed = 3f;
        giantRatBoss.SetActive(true);
        giantRatCinematic.SetActive(false);
        Destroy(giantRat.GetComponent<Rigidbody2D>());
        player.GetComponent<Player>().actionAllowed = true;
        bossMusic.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        bossHealthBar.SetActive(true);
        followScript.cameraSpeed = 5f;
        bossMusic.name = "BGM";
        GameObject.Destroy(this.gameObject);
    }
}
