using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLoader : MonoBehaviour
{
	private AudioSource[] speakers;
	private AudioSource sfx;
	private AudioSource background;

	public AudioClip walkingGlass;
	public AudioClip walkingMetal;

	private bool firstLoad = true;

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		speakers = FindObjectsOfType<AudioSource>();

		foreach (AudioSource speaker in speakers)
		{
			if (speaker.tag == "SFX")
			{
				sfx = speaker;
			}

			if (speaker.tag == "Background")
			{
				background = speaker;
				if (firstLoad)
				{
					firstLoad = false;
					background.Play();
				}
			}
		}
	}

	public void PlayDialogue(AudioClip clip)
	{
		foreach (AudioSource speaker in speakers)
		{
			if (speaker.tag != "SFX" && speaker.tag != "Background")
			{
				speaker.PlayOneShot(clip);
			}
		}
	}

	public void PlaySFX(AudioClip clip)
	{
		sfx.PlayOneShot(clip);
	}
}
