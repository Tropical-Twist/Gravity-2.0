using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRaycaster : MonoBehaviour
{
	private static readonly int WALL_OBJECT_MASK = (1 << 6) | (1 << 7);

	private Outline selectedObject;
	private new Transform camera;

	void Start()
	{
		camera = Camera.main.transform;
	}

	void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		if (PlayerStats.CanSetPlayerGravity && Physics.Raycast(ray, out hit, Mathf.Infinity, WALL_OBJECT_MASK))
		{
			Outline objectHit = hit.transform.gameObject.GetComponent<Outline>();

			//200, 90, 90
			if ((objectHit.gameObject.layer == 6 && objectHit.tag != "Glass") || (objectHit.tag == "Object" && PlayerStats.CanSetObjectGravity))
			{
				if (selectedObject == null)
				{
					selectedObject = objectHit;
					selectedObject.enabled = true;
				}
				else if (selectedObject != objectHit)
				{
					selectedObject.enabled = false;
					selectedObject = objectHit;
					selectedObject.enabled = true;
				}
			}
			else if (selectedObject != null)
			{
				selectedObject.enabled = false;
				selectedObject = null;
			}
		}
		else if (selectedObject != null)
		{
			selectedObject.enabled = false;
			selectedObject = null;
		}
	}
}
