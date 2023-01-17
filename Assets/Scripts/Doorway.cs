using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
	[SerializeField] bool openOnProximity = true;
	[SerializeField] Transform leftDoor;
	[SerializeField] Transform rightDoor;

	[SerializeField] AudioClip open;
	[SerializeField] AudioClip close;

	private AudioLoader loader;

	private bool opened = false;
	private float position = 0;

	private void Awake()
	{
		loader = FindObjectOfType<AudioLoader>();
	}

	private void Update()
	{
		if (opened && position < 2.0f)
		{
			position += Time.deltaTime * 4;
			leftDoor.position -= leftDoor.right * Time.deltaTime * 4;
			rightDoor.position += leftDoor.right * Time.deltaTime * 4;
		}
		else if (!opened && position > 0.0f)
		{
			position -= Time.deltaTime * 4;
			leftDoor.position += leftDoor.right * Time.deltaTime * 4;
			rightDoor.position -= leftDoor.right * Time.deltaTime * 4;
		}
	}

	public void Open()
	{
		opened = true;
		loader.PlaySFX(open);
	}

	public void Close()
	{
		opened = false;
		loader.PlaySFX(close);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (openOnProximity && other.tag == "Player")
		{
			Open();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (openOnProximity && other.tag == "Player")
		{
			Close();
		}
	}
}
