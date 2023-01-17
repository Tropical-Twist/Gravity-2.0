using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
	[SerializeField] int unlockID = 0;
	[SerializeField] GameObject model;

	private void Update()
	{
		if (model != null) { model.transform.position += Vector3.up * Mathf.Sin(Time.time) * 0.005f; }
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayerStats.Unlock(unlockID);
		Destroy(model);
		StartCoroutine(Sink());
	}

	IEnumerator Sink()
	{
		float dist = 1.0f;
		while (dist > 0.0f)
		{
			dist -= Time.deltaTime;
			transform.Translate(Vector3.down * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
	}
}
