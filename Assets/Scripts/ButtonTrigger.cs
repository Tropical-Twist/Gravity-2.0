using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTrigger : MonoBehaviour
{
	public UnityEvent pressEvent;
	public UnityEvent unpressEvent;

	private void OnTriggerEnter(Collider other)
	{
		pressEvent.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		unpressEvent.Invoke();
	}
}
