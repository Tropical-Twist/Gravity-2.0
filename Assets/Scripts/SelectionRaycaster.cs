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
		if (PlayerStats.CanSetPlayerGravity && Physics.Raycast(ray, out hit, Mathf.Infinity, WALL_OBJECT_MASK) &&
			hit.transform.gameObject.TryGetComponent<Outline>(out Outline objectHit))
		{
			if (selectedObject == null)
			{
				selectedObject = objectHit;
				selectedObject.enabled = true;
			}
			else if (selectedObject != objectHit)
			{
				selectedObject.enabled = selectedObject.selected;
				selectedObject = objectHit;
				selectedObject.enabled = true;
			}
		}
		else if (selectedObject != null)
		{
			selectedObject.enabled = selectedObject.selected;
			selectedObject = null;
		}
	}

	public void Deselect(Outline obj)
	{
		obj.selected = false;
		obj.enabled = false;
	}
}
