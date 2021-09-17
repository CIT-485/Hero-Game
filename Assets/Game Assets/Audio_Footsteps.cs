using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Footsteps : MonoBehaviour
{
	public GameObject player;
	public AudioSource audioSrc;
	
	// Update is called once per frame
	void Update()
	{
		// HeroKnight is moving 
		if (Mathf.Abs(player.GetComponent<PlayerMovement>().inputX) > 0 &&
			player.GetComponent<PlayerMovement>().grounded)
		{
			if (!audioSrc.isPlaying)
			{
				audioSrc.Play();
			}
		}
		else
			audioSrc.Stop();
	}
}
