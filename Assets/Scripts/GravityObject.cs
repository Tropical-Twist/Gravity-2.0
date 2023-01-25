using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour
{
	private static readonly float MAX_VELOCITY = 25.0f;

	[SerializeField] private Vector3 startingDirection = Vector3.down;
	[SerializeField] private AudioSource audio;

	private Vector3 gravityDirection;
	private float gravityMagnitude = 0.0f;
	private float gravityForce = 9.8f;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.detectCollisions = true;

		gravityDirection = startingDirection;
	}

	private void FixedUpdate()
	{
		Vector3 velocity = Vector3.zero;

		gravityMagnitude = Mathf.Min(gravityMagnitude + gravityForce * Time.fixedDeltaTime, MAX_VELOCITY);
		velocity += gravityDirection * gravityMagnitude;

		rb.velocity = velocity;
	}

	public void ChangeGravity(Vector3 direction)
	{
		gravityDirection = direction;
		gravityMagnitude = 0.0f;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			audio.PlayOneShot(audio.clip, Mathf.Lerp(0.275f, 0.325f, rb.velocity.magnitude) * 10.0f);
		}
	}
}
