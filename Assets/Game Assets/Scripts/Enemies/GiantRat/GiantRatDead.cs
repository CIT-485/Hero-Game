using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GiantRatDead : MonoBehaviour
{
    public GameObject       giantRatDead;
    public GameObject       bossHealthBar;
    public GameObject       whiteCrossFade;
    public Light2D          amuletLight;
    public GameObject       emitLightPrefab;
    Camera                  m_cam;
    GameObject              player;
    bool                    turnOffMusic = false;
    bool                    fadeIn = false;
    bool                    fadeOut = false;
    bool                    turnOnLight = false;
    float                   waitTime = 0;
    float                   waitTimeLimit = 1;
    List<GameObject>        lightPrefabs = new List<GameObject>();
    void Start()
    {
        m_cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(Dead());
    }
    private void Update()
    {
        if (GetComponent<Animator>().enabled)
        {
            if (GetComponent<Animator>().speed < 50)
                GetComponent<Animator>().speed *= 1.007f;
            else
                GetComponent<Animator>().SetTrigger("End");
        }
        if (turnOnLight)
        {
            waitTime += Time.deltaTime;
            amuletLight.transform.localScale *= 1.004f;
            if (waitTime > waitTimeLimit)
            {
                waitTime = 0;
                waitTimeLimit *= 0.9f;
                GameObject emitLight = Instantiate(emitLightPrefab);
                emitLight.GetComponent<StickToObject>().obj = transform;
                emitLight.transform.Rotate(0, 0, Random.Range(0, 360));
                lightPrefabs.Add(emitLight);
            }
        }
        if (fadeIn)
        {
            whiteCrossFade.GetComponent<CanvasGroup>().alpha += Time.deltaTime / 0.5f;
        }
        if (fadeOut)
        {
            whiteCrossFade.GetComponent<CanvasGroup>().alpha -= Time.deltaTime / 1;
        }
        if (turnOffMusic)
        {
            GameObject.Find("BosssssMusic").GetComponent<AudioSource>().volume -= Time.deltaTime / 10;
        }
    }
    IEnumerator Dead()
    {
        turnOffMusic = true;
        bossHealthBar.GetComponent<Animator>().SetTrigger("FadeOut");
        CameraFollowObject followScript = m_cam.GetComponent<CameraFollowObject>();
        followScript.objectToFollow = giantRatDead.transform;
        followScript.positionOffset = new Vector2(0, 2);
        followScript.cameraSpeed = 0.5f;
        player.GetComponent<Player>().body2d.velocity = Vector3.zero;
        player.GetComponent<Player>().actionAllowed = false;
        player.GetComponent<Player>().Grounded = true;
        player.GetComponent<Player>().GetComponent<Animator>().SetInteger("AnimState", 0);
        amuletLight.gameObject.SetActive(false);
        yield return new WaitForSeconds(3);
        amuletLight.gameObject.SetActive(true);
        turnOnLight = true;
        yield return new WaitForSeconds(13);
        whiteCrossFade.SetActive(true);
        fadeIn = true;
        turnOnLight = false;
        yield return new WaitForSeconds(2);
        foreach (GameObject g in lightPrefabs)
            Destroy(g);
        lightPrefabs.Clear();
        Destroy(amuletLight.gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        fadeIn = false;
        fadeOut = true;
        yield return new WaitForSeconds(1.5f);
        fadeOut = false;
        yield return new WaitUntil(() => false);
        followScript.objectToFollow = player.transform;
        player.GetComponent<Player>().actionAllowed = true;
        followScript.cameraSpeed = 5f;
        GameObject.Destroy(this.gameObject);
    }
}
