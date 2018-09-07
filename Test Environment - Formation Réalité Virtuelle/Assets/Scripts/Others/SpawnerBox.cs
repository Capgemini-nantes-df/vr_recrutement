using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Titre : SpawnerBox
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Script Spécifique pour le GameObject SpawnBox.
///     Permet le Spawn d'objets en intéragissant avec celui-ci.
///     
/// </summary>

public class SpawnerBox : MonoBehaviour {

    public GameObject objectToSpawn;
    public GameObject stuffInTheBox;
    public GameObject pistonsInTheBox;
    public GameObject pistonsOutOfTheBox;

    public float spawnDelay = 1f;
    public int maxSpawn = 8;

    private float spawnDelayTimer = 0f;
    private int count = 0;

    private void Start()
    {
        spawnDelayTimer = 0f;
        count = 0;
    }

    //Action si un objet entre en collision avec
    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
        
        //On continue le script si l'objet en collision est une manette, qu'il peut grab un objet, que le delais d'attente avec le spawn précédent est passé et qu'il reste encore des objets à spawn
        if (CanGrab(grabbingController) && Time.time >= spawnDelayTimer && count < maxSpawn)
        {
            stuffInTheBox.transform.GetChild(count).gameObject.SetActive(false);

            //for instantiate a clone of a prefab gameobject
            //GameObject newObject = Instantiate(objectToSpawn);

            GameObject currentObj = pistonsInTheBox.transform.GetChild(0).gameObject;
            currentObj.transform.SetParent(pistonsOutOfTheBox.transform);

            currentObj.SetActive(true);
            currentObj.GetComponent<MeshCollider>().enabled = true;
            currentObj.GetComponent<Rigidbody>().useGravity = true;

            grabbingController.GetComponent<VRTK_InteractTouch>().ForceTouch(currentObj);
            grabbingController.AttemptGrab();
            spawnDelayTimer = Time.time + spawnDelay;
            count += 1;

            
        }

        
    }

    //Fonction permettant le replacement d'un piston dans la spawnBox (appeler en Event)
    private void PutOnePiston(GameObject piston)
    {
        piston.GetComponent<RespawnObject>().RespawnToStart();
        count -= 1;
        stuffInTheBox.transform.GetChild(count).gameObject.SetActive(true);
        piston.transform.SetParent(pistonsInTheBox.transform);
        piston.SetActive(false);
        piston.GetComponent<MeshCollider>().enabled = false;
        piston.GetComponent<Rigidbody>().useGravity = false;

    }

    //Action pour faire spawn un objet or de la boite
    public void RespawnPistonsOutOfTheBox()
    {
        int childCount = pistonsOutOfTheBox.transform.childCount;
        for(int i = 0; i<childCount; i++)
        {
                PutOnePiston(pistonsOutOfTheBox.transform.GetChild(0).gameObject);  
        }
    }

    //Action pour vérifier si la manette peut grab un objet
    private bool CanGrab(VRTK_InteractGrab grabbingController)
    {
        return (grabbingController && grabbingController.GetGrabbedObject() == null && grabbingController.IsGrabButtonPressed());
    }

}
