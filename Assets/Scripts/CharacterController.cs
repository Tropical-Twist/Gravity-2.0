using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
	[SerializeField]
	private TMPro.TMP_Text debugText;
	[SerializeField]
	private float walkAcceleration = 400.0f;
	[SerializeField]
	private float runAcceleration = 600.0f;
	[SerializeField]
	private float jumpForce = 200.0f;
	[SerializeField]
	private float decelerationFactor = 0.8f;
	[SerializeField]
	private float jumpCooldown = 0.1f;
	[SerializeField]
	private Transform hand;

	private Vector3 velocity = Vector3.zero;

	private bool grounded = false;
	private Quaternion startRot;
	private Quaternion endRot;
	private float rotateTimer = 0.0f;
	private float jumpTimer = 0.0f;
	private float cutsceneTimer = 0.0f;
	private Vector3 startPos;
	private Vector3 endPos;

	private Rigidbody rb;
	[SerializeField] private Transform groundCheck;
	[SerializeField] private Transform orientation;
	private new Transform camera;
	private LineRenderer lineRenderer;

	private GravityObject selectedObject;

	[SerializeField] private AudioSource walking;
	[SerializeField] private AudioSource landing;
	[SerializeField] private AudioSource falling;
	[SerializeField] private AudioClip device_firing;
	[SerializeField] private AudioClip grav_change;

	private AudioLoader audioLoader;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.detectCollisions = true;

		camera = Camera.main.transform;
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		audioLoader = FindObjectOfType<AudioLoader>();

		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level0")) transform.root.GetChild(0).GetChild(0).gameObject.SetActive(false);
	}

	void FixedUpdate()
	{
		jumpTimer -= Time.fixedDeltaTime;
		rotateTimer -= Time.fixedDeltaTime;

		grounded = Physics.Raycast(groundCheck.position, -transform.up, out _, 0.1f, LayerMask.GetMask("Wall", "Object"));

		if (cutsceneTimer > 0.0f) CutsceneMovement();
		else if (PlayerStats.CanMove) StandardMovement();
		else Debug.LogError("Player is not in a cutscene but movement is not enabled.");

		if (rotateTimer >= 0.0f)
		{
			rotateTimer -= Time.fixedDeltaTime;
			//tranform.LookAt()? 
			transform.rotation = Quaternion.Slerp(endRot, startRot, Mathf.Max(rotateTimer, 0.0f));
		}

		if (!grounded && !falling.isPlaying)
		{
			falling.Play();
		}
		else if (grounded)
		{
			falling.Stop();
		}
	}

	public Vector3 GetGravityDirection(RaycastHit hit)
	{
		switch (hit.transform.tag)
		{
			case "Ground": return -hit.normal;
			case "Anti-Ground": return hit.normal;
			case "Mirror-Ground":
				Physics.Raycast(hit.transform.position, hit.transform.forward, out RaycastHit newHit, Mathf.Infinity, LayerMask.GetMask("Ground"));
				return GetGravityDirection(newHit);
			case "Glass": return Vector3.one;
			//case "Anti-Gravity": return Vector3.zero; Not currently possible
			case "Directional":
				switch (hit.transform.gameObject.GetComponent<DirectionalWall>().direction)
				{
					case DirectionalWall.Direction.UP: return Vector3.up;
					case DirectionalWall.Direction.DOWN: return Vector3.down;
					case DirectionalWall.Direction.NORTH: return Vector3.forward;
					case DirectionalWall.Direction.SOUTH: return Vector3.back;
					case DirectionalWall.Direction.EAST: return Vector3.right;
					case DirectionalWall.Direction.WEST: return Vector3.left;
				}
				break;
			case "Untagged": return Vector3.one;
		}

		Debug.LogError($"Unkown ground type: {hit.transform.tag}");
		return Vector3.one;
	}

	private bool wasShooting = false;
	public void StandardMovement()
	{
		//update Audio
		if (grounded && rb.velocity.sqrMagnitude > 0.1f && !walking.isPlaying)
		{
			walking.Play();
		}
		else if (!grounded || rb.velocity.sqrMagnitude <= 0.1f)
		{
			walking.Stop();
		}

		// Show line
		Physics.Raycast(camera.position, camera.forward, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Wall"));
		if (PlayerStats.CanSetPlayerGravity && Input.GetButton("Fire1") && hit.collider != null)
		{
			wasShooting = true;
			lineRenderer.enabled = true;
			lineRenderer.SetPosition(0, hand.position);
			lineRenderer.SetPosition(1, hit.point);
		}
		// Change gravity
		else if (wasShooting)
		{
			if (hit.collider == null) Debug.Break(); // This is here because it will sometimes happen very rarely but it's hard to catch.
			wasShooting = false;
			lineRenderer.enabled = false;
			Vector3 direction = GetGravityDirection(hit);

			if (direction != Vector3.one)
			{
				AudioSource.PlayClipAtPoint(grav_change, transform.position, 1.0f);
				//gravityDirection = direction;
				startRot = transform.rotation;
				endRot = Quaternion.FromToRotation(Vector3.up, -direction);
				rotateTimer = 1.0f;
			}
		}

		// Change an object's gravity
		if (PlayerStats.CanSetObjectGravity && Input.GetButtonDown("Fire2") &&
			Physics.Raycast(camera.position, camera.forward, out hit, Mathf.Infinity, LayerMask.GetMask("Wall", "Object")))
		{
			AudioSource.PlayClipAtPoint(device_firing, transform.position, 0.3f);
			if (hit.transform.tag == "Object")
			{
				if (selectedObject != null) { Camera.main.GetComponent<SelectionRaycaster>().Deselect(selectedObject.GetComponent<Outline>()); }
				hit.transform.gameObject.TryGetComponent<GravityObject>(out selectedObject);
				selectedObject.GetComponent<Outline>().selected = true;
			}
			else if (selectedObject != null)
			{
				Vector3 direction = GetGravityDirection(hit);
				if (direction != Vector3.zero)
				{
					selectedObject.ChangeGravity(direction);
				}
			}
		}

		// Jump
		if (grounded && jumpTimer < 0.0f && Input.GetAxis("Jump") > 0.0f)
		{
			rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
			jumpTimer = jumpCooldown;
		}

		// Sprint
		float acceleration;
		if (Input.GetKey(KeyCode.LeftShift)) { acceleration = runAcceleration; }
		else { acceleration = walkAcceleration; }

		// Lateral Movement
		float sideSpeed = Input.GetAxis("Horizontal");
		float forwardSpeed = Input.GetAxis("Vertical");
		// Apply lateral force.
		rb.AddRelativeForce(new Vector3(sideSpeed, 0, forwardSpeed).normalized * acceleration * Time.fixedDeltaTime, ForceMode.Force);

		// Gravity
		rb.AddRelativeForce(Physics.gravity, ForceMode.Force);

		// Apply more drag if there is no input so user slows faster.
		if (Mathf.Abs(sideSpeed) < float.Epsilon && Mathf.Abs(forwardSpeed) < float.Epsilon && grounded)
		{
			rb.velocity *= decelerationFactor;
		}
	}

	public void CutsceneMovement()
	{
		cutsceneTimer -= Time.fixedDeltaTime;
		if (cutsceneTimer > 0)
		{
			//camera.localRotation = Quaternion.Slerp(endCamX, startCamX, Mathf.Max(cutsceneTimer, 0.0f));
			//camera.parent.localRotation = Quaternion.Slerp(endCamY, startCamY, Mathf.Max(cutsceneTimer, 0.0f));
			transform.rotation = Quaternion.Slerp(endRot, startRot, cutsceneTimer);
			transform.position = Vector3.Lerp(endPos, startPos, cutsceneTimer);
		}
		else
		{
			//camera.localRotation = endCamX;
			//camera.parent.localRotation = endCamY;
			transform.rotation = endRot;
			transform.position = endPos;
			PlayerStats.CanLook = true;
			PlayerStats.CanMove = true;
		}
		velocity = Vector3.zero;
		rb.velocity = velocity;
	}

	public void SetCharacter(Transform endTransform, float cutsceneTime)
	{
		startRot = transform.rotation;
		endRot = endTransform.rotation;
		startPos = transform.position;
		endPos = endTransform.position;
		//startCamX = camera.localRotation;
		//endCamX = Quaternion.Euler(Vector3.right * camX);
		//startCamY = camera.parent.localRotation;
		//endCamY = Quaternion.Euler(Vector3.up * camY);

		cutsceneTimer = cutsceneTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		Ray ray = new Ray(transform.position, -transform.up);
		if (collision.collider.bounds.IntersectRay(ray))
		{
			if (audioLoader != null)
			{

				if (collision.gameObject.tag == "Glass")
				{
					walking.clip = audioLoader.walkingGlass;
					landing.clip = audioLoader.walkingGlass;
				}
				else
				{
					walking.clip = audioLoader.walkingMetal;
					landing.clip = audioLoader.walkingMetal;
				}
				landing.PlayOneShot(landing.clip, Mathf.Lerp(0.275f, 0.325f, rb.velocity.magnitude) * 10.0f);
			}
		}
	}
}