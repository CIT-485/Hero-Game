using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip[] clip;

	[Range(0f, 1f)]
	public float volume = 1.0f;
	[Range(0f, 1.5f)]
	public float pitch = 1.0f;
	[Range(0f, 5f)]
	public float fadeTime = 1.0f;

	public Vector2 randomVolume = new Vector2(1.0f, 1.0f);
	public Vector2 randomPitch = new Vector2(1.0f, 1.0f);

	public bool loop = false;

	[HideInInspector]
	public AudioSource source;

	public void SetSource (AudioSource _source)
    {
		source = _source;
		int randomClip = Random.Range(0, clip.Length - 1);
		source.clip = clip[randomClip];
	}

	public void Play ()
    {
		if (clip.Length > 1)
		{
			int randomClip = Random.Range(0, clip.Length - 1);
			source.clip = clip[randomClip];
		}

		source.volume = volume * Random.Range(randomVolume.x, randomVolume.y);
		source.pitch = pitch * Random.Range(randomPitch.x, randomPitch.y);
		source.loop = loop;
		source.Play();
	}

	public void Stop()
    {
		source.Stop();
    }
}

public class AudioPlayer : MonoBehaviour
{
	public static AudioPlayer	instance;

	[SerializeField]
	private Sound[]				sounds;
	private Player				player;
	
	private bool				isWalking = false;
	private bool				isJumping = false;
	private bool				isLanded = false;
	private bool				isAttacking = false;

	private float				playFootstepTime = 0;

	private void Awake()
    {
        if(instance != null)
        {
			Debug.LogError("More than one AudioManager in the scene.");
        } else
        {
			instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
			GameObject _go = new GameObject("Sound_" + i + " " + sounds[i].name);
			//_go.transform.SetParent(this.transform);
			_go.transform.SetParent(transform);
			sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void LateUpdate()
	{
		if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isDead)
		{
			playJump();
			playSword();
		}
		playFootsteps();
		playLand();
	}

    private void playFootsteps()
	{

		if ((player.inputX > 0 || player.inputX < 0) && isWalking == false && player.grounded == true && !player.rolling)
		{
			playFootstepTime += Time.deltaTime;
			if (playFootstepTime > 0.25f)
			{
				PlaySound("Footstep");
				isWalking = true;
				//StartCoroutine("FootstepsTimer");
			}
		}
		else if (!player.actionAllowed)
		{
			playFootstepTime = 0;
			StopSound("Footstep");
        }
		else if (player.inputX == 0 || isWalking == false || !player.grounded || player.rolling)
		{
			playFootstepTime = 0;
			FadeOutSound("Footstep");
			isWalking = false;
		}
	}

	private void playJump()
    {
		if (Input.GetKeyDown("space") && isJumping == false)
		{
			//PlaySound("Jump");
			isJumping = true;
			isLanded = false;
			isWalking = false;
		}
	}

	private void playLand()
	{
		if (player.grounded == true && isLanded == false)
		{
			//PlaySound("Land");
			isLanded = true;
			isJumping = false;
		}
	}

	private void playSword()
    {
		if (!isAttacking && player.attacking && (Input.GetMouseButtonDown(0) || Input.GetKeyDown("k"))) {
			isAttacking = true;
			PlaySound("Sword swing");
		}
		else if (!player.attacking)
        {
			isAttacking = false;
        }
	}

	private void playDeath()
    {
		PlaySound("Death");
	}

	/*
	 * I removed this since it created the meshing noise when the player rapidly pressed left and right
	IEnumerator FootstepsTimer()
    {
        while (isWalking == true)
        {
			yield return new WaitForSeconds(0.4f);
		}
    }
	*/
	public
	IEnumerator FadeOut(Sound sound)
	{
		while (sound.volume > 0)
		{
			sound.source.volume -= sound.volume * Time.deltaTime / sound.fadeTime;
			yield return null;
		}
		sound.Stop();
		sound.source.volume = sound.volume;
	}
	public void FadeOutSound(string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				IEnumerator fade = FadeOut(sounds[i]);
				StartCoroutine(fade);
				StopCoroutine(fade);
				return;
			}
		}
	}
	public void PlaySound (string _name)
    {
		for (int i = 0; i < sounds.Length; i++)
        {
			if (sounds[i].name == _name)
            {
				sounds[i].Play();
				return;
            }
        }

		// no sounds with _name
		Debug.LogWarning("AudioManager: Sound not found in list, " + _name);
    }
	public void StopSound (string _name)
	{
		for (int i = 0; i < sounds.Length; i++)
		{
			if (sounds[i].name == _name)
			{
				sounds[i].Stop();
				return;
			}
		}
	}
}
