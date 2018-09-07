using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Titre : Obj Snap Highlight Helper Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     NOT FINISHED == DON'T USE IT FOR NOW
///     
///     Permet à un gameObject de suivre le transform (Position et rotation, =/= scale) d'un gameObject associé.
///     
/// </summary>

public class FollowTransformScript : MonoBehaviour {


    [Header("Object to Follow Settings")]
    public Transform objectToFollow;

    private bool initialisation;

    private float posx;
    private float posy;
    private float posz;

    private float rotx;
    private float roty;
    private float rotz;

    private float fposx;
    private float fposy;
    private float fposz;

    private float frotx;
    private float froty;
    private float frotz;

    private Vector3 posOffset;
    private Vector3 rotOffset;

    // Use this for initialization
    void Start()
    {
        initialisation = true;
    }

    // Update is called once per frame
    void Update()
    {

        //follow target GameObject 
        if (this.gameObject.activeSelf == true && initialisation == false)
        {

            //transform.position = new Vector3((objectToFollow.transform.position.x - fposx) + posx, (objectToFollow.transform.position.y - fposy) + posy, 
            //(objectToFollow.transform.position.z - fposz) + posz);

            //transform.rotation = Quaternion.Euler((objectToFollow.transform.rotation.eulerAngles.x - frotx) + rotx, (objectToFollow.transform.rotation.eulerAngles.y - froty) + roty,
            //(objectToFollow.transform.rotation.eulerAngles.z + frotz) - rotz);

            transform.position = objectToFollow.transform.position + posOffset;
            transform.eulerAngles = objectToFollow.eulerAngles + rotOffset;

        }


        //Set innitial position of Gameobject
        else if (this.gameObject.activeSelf == true && initialisation == true)
        {
            posOffset = transform.position - objectToFollow.transform.position;
            rotOffset = transform.eulerAngles - objectToFollow.eulerAngles;

            /*posx = transform.position.x;
            posy = transform.position.y;
            posz = transform.position.z;

            rotx = transform.rotation.eulerAngles.x;
            roty = transform.rotation.eulerAngles.y;
            rotz = transform.rotation.eulerAngles.z;

            fposx = objectToFollow.transform.position.x;
            fposy = objectToFollow.transform.position.y;
            fposz = objectToFollow.transform.position.z;

            frotx = objectToFollow.transform.rotation.eulerAngles.x;
            froty = objectToFollow.transform.rotation.eulerAngles.y;
            frotz = objectToFollow.transform.rotation.eulerAngles.z;*/

            initialisation = false;
        }

    }
}
