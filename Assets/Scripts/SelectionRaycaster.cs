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
		if (Physics.Raycast(camera.position, camera.forward, out hit, Mathf.Infinity, WALL_OBJECT_MASK))
		{
			Transform objectHit = hit.transform;
			if (objectHit.TryGetComponent(out Outline outline))
			{
				//200, 90, 90
				//TODO: Check for wall or object, walls get OutlineAll and objects get OutlineAndSilhouette
				if (selectedObject == null)
				{
					selectedObject = outline;
					selectedObject.OutlineColor = Color.red;
					selectedObject.OutlineMode = Outline.Mode.OutlineAll;
					selectedObject.OutlineWidth = 5.0f;
				}
				else if(selectedObject != objectHit)
				{
					selectedObject.OutlineMode = Outline.Mode.OutlineVisible;
					selectedObject.OutlineWidth = 0.0f;
					selectedObject = outline;
					selectedObject.OutlineColor = Color.red;
					selectedObject.OutlineMode = Outline.Mode.OutlineAll;
					selectedObject.OutlineWidth = 5.0f;
				}
			}
			else if(selectedObject != null)
			{
				selectedObject.OutlineMode = Outline.Mode.OutlineVisible;
				selectedObject.OutlineWidth = 0.0f;
				selectedObject = null;
			}
		}
		else if (selectedObject != null)
		{
			selectedObject.OutlineMode = Outline.Mode.OutlineVisible;
			selectedObject.OutlineWidth = 0.0f;
			selectedObject = null;
		}
	}
}
