using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
	[SerializeField] private AudioClip clip;
	[SerializeField] private float delay = 0.0f;
	[SerializeField] private bool playOnce = true;
	[SerializeField] private bool useSpeakers = false;

	private AudioLoader loader;
	private bool played = false;

	private void Awake()
	{
		loader = FindObjectOfType<AudioLoader>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!played)
		{
			played = playOnce;

			StartCoroutine(Wait(delay));
			if (useSpeakers) { loader.PlayDialogue(clip); }
			else { loader.PlaySFX(clip); }
		}
	}

	IEnumerator Wait(float amt)
	{
		yield return new WaitForSeconds(amt);
	}
}
