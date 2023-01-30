using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseHazard : MonoBehaviour
{
    [SerializeField] GameObject hazard;
    bool activated;

    private void OnTriggerEnter(Collider other)
    {
        if(hazard.activeInHierarchy == true && activated == false)
        {
            hazard.SetActive(false);
            activated = true;
            Debug.Log("Activated false");
        }
        else if(hazard.activeInHierarchy == false && activated == false)
        {
            hazard.SetActive(true);
            activated = true;
            Debug.Log("Activated true");
        }
    }
}
