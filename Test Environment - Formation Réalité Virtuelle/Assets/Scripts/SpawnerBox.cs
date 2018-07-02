﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawnerBox : MonoBehaviour {

    public GameObject objectToSpawn;
    public GameObject stuffInTheBox;
    public GameObject grabbablePistons;
    public float spawnDelay = 1f;
    public int maxSpawn = 8;

    private float spawnDelayTimer = 0f;
    private int count = 0;

    private void Start()
    {
        spawnDelayTimer = 0f;
        count = 0;
    }

    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
        if (CanGrab(grabbingController) && Time.time >= spawnDelayTimer && count < maxSpawn)
        {
            stuffInTheBox.transform.GetChild(count).gameObject.SetActive(false);

            //for instantiate a clone of a prefab gameobject
            //GameObject newObject = Instantiate(objectToSpawn);

            grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(grabbablePistons.transform.GetChild(count).gameObject);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
            count += 1;

            
        }

        
    }

    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.IsGrabButtonPressed());
    }

}
