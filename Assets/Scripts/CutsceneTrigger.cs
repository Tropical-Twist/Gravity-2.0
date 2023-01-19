using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
	[SerializeField] private Transform endTransform;
	[SerializeField] private float cameraRotationX;
	[SerializeField] private float cameraRotationY;
	[SerializeField] private float duration = 1;
	[SerializeField] private bool canMove = false;
	[SerializeField] private bool canLook = false;
	[SerializeField] private bool canSkip = false;
	[SerializeField] private KeyCode skipButton;

	private bool played = false;
	private Coroutine runScene = null;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player") && !played)
		{
			var player = other.GetComponent<CharacterController>();
			played = true;
			PlayerStats.CanLook = canLook;
			PlayerStats.CanMove = canMove;

			player.SetCharacter(endTransform, duration);

			//runScene = StartCoroutine(Wait(duration));
		}
	}

	//public void Update()
	//{
	//	if (runScene != null && canSkip && Input.GetKeyDown(skipButton))
	//	{
	//		StopCoroutine(runScene);
	//		runScene = null;
	//		PlayerStats.CanLook = true;
	//		PlayerStats.CanMove = true;
	//	}
	//}

	//IEnumerator Wait(float amt)
	//{
	//	yield return new WaitForSeconds(amt);
	//	runScene = null;
	//	PlayerStats.CanLook = true;
	//	PlayerStats.CanMove = true;
	//}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(endTransform.position, endTransform.position + endTransform.right * 0.5f);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(endTransform.position, endTransform.position + endTransform.up * 0.5f);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(endTransform.position, endTransform.position + endTransform.forward * 0.5f);
		Gizmos.color = Color.white;
		Gizmos.DrawSphere(endTransform.position, 0.1f);
	}
}
