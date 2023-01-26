using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioLoader : MonoBehaviour
{
	private AudioSource[] speakers;
	private AudioSource sfx;
	private AudioSource background;
	private AudioSource music;

	public AudioClip walkingGlass;
	public AudioClip walkingMetal;
	public AudioClip[] musicList;

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

			if (speaker.tag == "Music")
			{
				music = speaker;

				music.clip = musicList[scene.buildIndex];
				music.Play();
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
