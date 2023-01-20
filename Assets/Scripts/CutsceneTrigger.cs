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
	[SerializeField] private KeyCode skipButton;

	private bool played = false;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Player") && !played)
		{
			var player = other.GetComponent<CharacterController>();
			played = true;
			PlayerStats.CanLook = canLook;
			PlayerStats.CanMove = canMove;

			player.SetCharacter(endTransform, duration);
		}
	}

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
