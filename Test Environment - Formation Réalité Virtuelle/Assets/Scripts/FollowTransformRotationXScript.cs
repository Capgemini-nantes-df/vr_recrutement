using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformRotationXScript : MonoBehaviour {

    [Header("Object to Follow Settings")]
    public GameObject objectToFollow;

    private bool initialisation;

    // Use this for initialization
    void Start()
    {
        initialisation = true;
    }

    // Update is called once per frame
    void Update () {

        if (this.gameObject.activeSelf == true && initialisation == false)
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, objectToFollow.transform.eulerAngles.y, transform.eulerAngles.z); ;
        }

        else if (this.gameObject.activeSelf == true && initialisation == true)
        {
            //Debug.Log("initalisation du cylindre Rotatif rotation y à la rotation de l'objet cible");
            objectToFollow.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            initialisation = false;
        }
        
    }

   
       

}
