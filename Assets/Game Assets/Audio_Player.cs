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

	public Vector2 randomVolume = new Vector2(1.0f, 1.0f);
	public Vector2 randomPitch = new Vector2(1.0f, 1.0f);

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
		source.Play();
	}

}

public class Audio_Player : MonoBehaviour
{
	public static Audio_Player instance;

	[SerializeField]
	Sound[] sounds;
	public PlayerMovement movement;
	public PlayerCombat combat;

	public bool isWalking = false;
	public bool isJumping = false;
	public bool isLanded = false;

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

		movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		combat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCombat>();
    }

	private void playFootsteps()
    {
		if ((movement.inputX > 0 || movement.inputX < 0) && isWalking == false && movement.grounded == true)
		{
			isWalking = true;
			StartCoroutine("FootstepsTimer");
		}
		else if (movement.inputX == 0)
		{
			isWalking = false;
		}
	}

	private void playJump()
    {
		if (Input.GetKeyDown("space") && isJumping == false)
		{
			isJumping = true;
			PlaySound("Jump");
			isLanded = false;
		}
		else if (movement.grounded == true && isLanded == false)
		{
			isLanded = true;
			PlaySound("Jump");
			isJumping = false;
		}
	}

	private void playSword()
    {
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("k")) {
			PlaySound("Sword swing");
		}
	}

    private void Update()
    {
		playFootsteps();
		playJump();
		playSword();
    }

	IEnumerator FootstepsTimer()
    {
        while (isWalking == true)
        {
			PlaySound("Footstep");
			yield return new WaitForSeconds(0.3f);
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
}