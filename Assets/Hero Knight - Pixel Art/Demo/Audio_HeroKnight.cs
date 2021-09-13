using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_HeroKnight : MonoBehaviour
{
	// HeroKnight directional movement variable
	float dirX;
	[SerializeField]
	// Movement speed variable
	float moveSpeed = 4.0f;
	Rigidbody2D rb;
	AudioSource audioSrc;
	bool isMoving = false;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		audioSrc = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update()
	{
		// dirX is set to left or right depending on if the left or right arrow is pressed on the keyboard mutiplied by movement speed
		dirX = Input.GetAxis("Horizontal") * moveSpeed;

		// HeroKnight is moving 
		if (rb.velocity.x != 0)
			isMoving = true;
		else
			isMoving = false;
		// Play audio sound if HeroKnight is moving
		if (isMoving)
		{
			if (!audioSrc.isPlaying)
				audioSrc.Play();
		}
		else
			audioSrc.Stop();
	}

	void FixedUpdate()
	{
		rb.velocity = new Vector2(dirX, rb.velocity.y);
	}
}
