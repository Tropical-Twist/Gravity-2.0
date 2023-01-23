using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRaycaster : MonoBehaviour
{
	private static readonly int WALL_OBJECT_MASK = (1 << 6) | (1 << 7);

	private GameObject selectedObject;
	private new Transform camera;

	void Start()
	{
		camera = Camera.main.transform;
	}

	void Update()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, WALL_OBJECT_MASK))
		{
			Transform objectHit = hit.transform;

			//200, 90, 90
			if (objectHit.gameObject.layer == 6)
			{
				if (selectedObject == null)
				{
					selectedObject = objectHit.gameObject;
					Outline ol = selectedObject.AddComponent<Outline>();
					ol.OutlineColor = Color.red;
					ol.OutlineMode = Outline.Mode.OutlineAll;
					ol.OutlineWidth = 5.0f;
				}
				else if (selectedObject != objectHit.gameObject)
				{
					Destroy(selectedObject.GetComponent<Outline>());
					selectedObject = objectHit.gameObject;
					Outline ol = selectedObject.AddComponent<Outline>();
					ol.OutlineColor = Color.red;
					ol.OutlineMode = Outline.Mode.OutlineAll;
					ol.OutlineWidth = 5.0f;
				}
			}
			else if(objectHit.tag == "Object")
			{
				if(selectedObject == null)
				{
					selectedObject = objectHit.gameObject;
					Outline ol = selectedObject.AddComponent<Outline>();
					ol.OutlineColor = Color.red;
					ol.OutlineMode = Outline.Mode.OutlineAndSilhouette;
					ol.OutlineWidth = 5.0f;
				}
				else if (selectedObject != objectHit.gameObject)
				{
					Destroy(selectedObject.GetComponent<Outline>());
					selectedObject = objectHit.gameObject;
					Outline ol = selectedObject.AddComponent<Outline>();
					ol.OutlineColor = Color.red;
					ol.OutlineMode = Outline.Mode.OutlineAndSilhouette;
					ol.OutlineWidth = 5.0f;
				}
			}
			else if (selectedObject != null)
			{
				Destroy(selectedObject.GetComponent<Outline>());
				selectedObject = null;
			}
		}
		else if (selectedObject != null)
		{
			Destroy(selectedObject.GetComponent<Outline>());
			selectedObject = null;
		}
	}
}
