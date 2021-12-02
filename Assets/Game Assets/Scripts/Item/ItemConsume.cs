using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsume : MonoBehaviour
{
    public GameObject healParticles;
    private Player player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void PotionHeal(int amount)
    {
        player.healthBar.Healing(amount);
        player.healFlash.SetActive(true);
        player.healFlash.GetComponent<Light2DFade>().Fade(1.25f);
        StartCoroutine(FadeParticle(Instantiate(healParticles, player.transform).GetComponent<ParticleSystem>()));
    }
    public void GetAmulet()
    {
        player.hasAmulet = true;
        player.playerCanvas.GetComponent<Animator>().SetTrigger("FirstTime");
        GameObject.Find("GiantRatDeadCinematic").GetComponent<GiantRatDead>().exit.SetActive(false);
    }
    IEnumerator FadeParticle(ParticleSystem healParticles)
    {
        yield return new WaitForSeconds(2);
        healParticles.GetComponent<ParticleFade>().Fade();
        Destroy(gameObject);
    }
}