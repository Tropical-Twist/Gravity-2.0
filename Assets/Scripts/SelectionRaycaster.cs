using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionRaycaster : MonoBehaviour
{
	private static readonly int GROUND_OBJECT_MASK = (1 << 3) | (1 << 10) | (1 << 13);

	private Highlight selectedObject;
	private new Transform camera;

	void Start()
    {
		camera = Camera.main.transform;
    }

    void Update()
    {
		RaycastHit hit;
		if (Physics.Raycast(camera.position, camera.forward, out hit, Mathf.Infinity, GROUND_OBJECT_MASK))
		{
			Transform objectHit = hit.transform;
			if(objectHit.GetComponent<Highlight>() != null)
			{
				if(selectedObject != objectHit && selectedObject != null)
				{
					selectedObject.OnDeselect();
					selectedObject = null;
				}
				if(selectedObject == null)
				{
					selectedObject = objectHit.GetComponent<Highlight>();
					selectedObject.OnSelect();
				}
			}
		}
		else if(selectedObject != null)
		{
			selectedObject.OnDeselect();
			selectedObject = null;
		}
	}
}
