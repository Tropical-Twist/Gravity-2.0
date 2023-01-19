using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
	private Camera mainCamera;
	private new Camera camera;

    void Awake()
    {
		mainCamera = Camera.main;
		camera = GetComponent<Camera>();
    }

    void Update()
    {
        camera.fieldOfView = mainCamera.fieldOfView;
		camera.transform.position = mainCamera.transform.position;
		camera.transform.rotation = mainCamera.transform.rotation;
    }
}
