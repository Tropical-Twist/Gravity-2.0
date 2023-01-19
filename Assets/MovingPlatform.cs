using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
public class MovingPlatform : MonoBehaviour
{
	[Tooltip("This gameobject can be added to its own list of waypoints. If this is the case then that will be made the new starting point and a new transform will be created at that position.")]
	public Transform[] waypoints;
	public int startingWayPoint = 0;
	public float speed = 1;
	[Tooltip("Do not touch this when in Bounce mode.")]
	public bool reverseOrder = false;
	public EndpointBehavior endpointBehavior;
	public bool drawGizmos = true;

	public enum EndpointBehavior
	{
		Bounce, Wrap, Stop
	}

	private Rigidbody rb;
	private int currentWaypoint = 0;
	private float threshhold = 0.1f;
	private float sqrThreshhold;
	private Vector3 target;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		sqrThreshhold = threshhold * threshhold;
		currentWaypoint = startingWayPoint;
		for (int i = 0; i < waypoints.Length; i++)
		{
			if (waypoints[i] == transform)
			{
				var gameObject = new GameObject("Starting Waypoint");
				gameObject.transform.parent = transform.parent;
				gameObject.transform.position = transform.position;
				waypoints[i] = gameObject.transform;
				currentWaypoint = i;
				break;
			}
		}
		if (endpointBehavior == EndpointBehavior.Bounce)
		{
			if (currentWaypoint == 0)
			{
				reverseOrder = false;
				currentWaypoint = 1;
			}
			if (currentWaypoint == waypoints.Length - 1)
			{
				reverseOrder = true;
				currentWaypoint = waypoints.Length - 2;
			}
		}
	}

	private void FixedUpdate()
	{
		float sqrDistance = (rb.position - waypoints[currentWaypoint].position).sqrMagnitude;
		if (sqrDistance > sqrThreshhold)
		{
			target = (waypoints[currentWaypoint].position - rb.position).normalized;
			rb.MovePosition(rb.position + (target * speed * Time.fixedDeltaTime));
		}
		else
		{
			int nextWaypoint = int.MaxValue;
			switch (endpointBehavior)
			{
				case EndpointBehavior.Bounce:
					if (currentWaypoint == 0 || currentWaypoint == waypoints.Length - 1) reverseOrder = !reverseOrder;
					nextWaypoint = (reverseOrder) ? currentWaypoint - 1 : currentWaypoint + 1;
					if (nextWaypoint < 0 || nextWaypoint > waypoints.Length - 1)
					{
						Debug.LogError("The next waypoint index was out of range. This can happen if you modify reverse order while the object is between the second to last and last waypoints in bounce mode.");
						nextWaypoint = currentWaypoint;
					}
					break;
				case EndpointBehavior.Wrap:
					nextWaypoint = (currentWaypoint + (reverseOrder ? -1 : 1)) % waypoints.Length;
					if (nextWaypoint == -1) nextWaypoint = waypoints.Length - 1;
					break;
				case EndpointBehavior.Stop:
					if (currentWaypoint == 0 && reverseOrder
						|| currentWaypoint == waypoints.Length - 1 && !reverseOrder)
					{ nextWaypoint = currentWaypoint; }
					else nextWaypoint = (reverseOrder) ? currentWaypoint - 1 : currentWaypoint + 1;
					break;
				default:
					Debug.Log("endpointBehavior was not set.");
					Debug.Break();
					break;
			}

			currentWaypoint = nextWaypoint;
		}
	}

	private void OnDrawGizmos()
	{
		if (!drawGizmos) return;
		for (int i = 0; i < waypoints.Length; i++)
		{
			Gizmos.DrawSphere(waypoints[i].position, threshhold);
			if (endpointBehavior == EndpointBehavior.Wrap)
			{
				Gizmos.DrawLine(waypoints[i].position, waypoints[(i + 1) % waypoints.Length].position);
			}
			else if (i != waypoints.Length - 1)
			{
				Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
			}
		}
	}
}
