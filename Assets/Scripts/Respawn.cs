using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Respawn : MonoBehaviour
{
    private GameMaster gm;
    public ParticleSystem psConsist;
    public ParticleSystem psStart;
    public Light2D bonfireLight;
    private bool doneIntensity;
    private bool doneRadius;
    private bool playedOnce;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    private void Update()
    {
        if (bonfireLight.enabled)
        {
            if (doneIntensity)
            {
                if (bonfireLight.intensity > 0.5)
                    bonfireLight.intensity -= Mathf.Abs(bonfireLight.intensity - 0.4f) * Time.deltaTime / 3f;
            }
            else
            {
                if (bonfireLight.intensity < 1)
                    bonfireLight.intensity += Mathf.Abs(bonfireLight.intensity - 1.1f) * Time.deltaTime / 0.5f;
                else
                    doneIntensity = true;
            }

            if (doneRadius)
            {
                if (bonfireLight.pointLightOuterRadius > 1.5f)
                {
                    bonfireLight.pointLightOuterRadius -= Mathf.Abs(bonfireLight.pointLightOuterRadius - 1.4f) * Time.deltaTime / 3f;
                    bonfireLight.pointLightInnerRadius -= Mathf.Abs(bonfireLight.pointLightInnerRadius - 0.4f) * Time.deltaTime / 3f;
                }
            }
            else
            {
                if (bonfireLight.pointLightOuterRadius < 2.75)
                {
                    bonfireLight.pointLightOuterRadius += Mathf.Abs(bonfireLight.pointLightOuterRadius - 2.85f) * Time.deltaTime / 0.75f;
                    bonfireLight.pointLightInnerRadius += Mathf.Abs(bonfireLight.pointLightInnerRadius - 1.6f) * Time.deltaTime / 0.75f;
                }
                else
                    doneRadius = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Bonfire());
            gm.lastRespawnPos = transform.position;
        }
    }
    IEnumerator Bonfire()
    {
        psStart.gameObject.SetActive(true);
        if (!playedOnce)
            psStart.Play();
        bonfireLight.enabled = true;
        playedOnce = true;
        GetComponent<Animator>().SetTrigger("Active");
        yield return new WaitForSeconds(0.2f);
        psConsist.gameObject.SetActive(true);
        if (!psConsist.isPlaying)
            psConsist.Play();
    }
}
