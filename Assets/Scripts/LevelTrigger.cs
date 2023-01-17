using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
	[SerializeField] int nextLevel;
	[SerializeField] Doorway door;

	private SceneLoader loader;
	private CharacterController player;

	private void Awake()
	{
		loader = FindObjectOfType<SceneLoader>();
		player = FindObjectOfType<CharacterController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerStats.CanLook = true;
		PlayerStats.CanMove = false;

		if (door != null) { door.Close(); }

		player.SetCharacter(transform.position, transform.rotation, 0.0f, 0.0f);

		StartCoroutine(WaitForLevel(2.0f));
	}

	IEnumerator WaitForLevel(float amt)
	{
		yield return new WaitForSeconds(amt);

		loader.LoadLevel(nextLevel);
	}
}
