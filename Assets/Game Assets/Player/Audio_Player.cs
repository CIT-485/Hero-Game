using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
	public string name;
	public AudioClip[] clip;

	[Range(0f, 1f)]
	public float volume = 0.7f;
	[Range(0f, 1.5f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0.1f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.1f;

	private AudioSource source;

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
		source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomVolume / 2f));
		source.Play();
    }

}

public class Audio_Player : MonoBehaviour
{
	public static Audio_Player instance;

	[SerializeField]
	Sound[] sounds;
	public PlayerMovement movement;
	public bool isWalking = false;

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
			_go.transform.SetParent(this.transform);
			sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

		movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		// delete this line of code
		// PlaySound("Footstep");
    }

    private void Update()
    {
        if((movement.inputX > 0 || movement.inputX < 0) && isWalking == false && movement.grounded == true)
        {
			isWalking = true;
			StartCoroutine("FootstepsTimer");
		} else if(movement.inputX == 0)
        {
			isWalking = false;
        }
    }

	IEnumerator FootstepsTimer()
    {
        while (isWalking == true)
        {
			PlaySound("Footstep");
			yield return new WaitForSeconds(0.4f);
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
