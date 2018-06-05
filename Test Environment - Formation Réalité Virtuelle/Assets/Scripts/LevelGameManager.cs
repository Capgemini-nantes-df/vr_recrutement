using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGameManager : MonoBehaviour {

    [Header("Motor Animation Settings")]

    public Animator motorAnim;

    public GameObject MotorPresentation;
    public GameObject MotorAssemblage;

    [Header("Lvl Game Settings")]

    public string snapDropTag;
    private GameObject[] allSnapDropZone;

    private int snapDropZoneCount;
    private int validSnapCount;
    private bool isFinished;


    // Use this for initialization
    void Start () {
        snapDropZoneCount = 0;
        isFinished = false;

        allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
        snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;
    }
	
	// Update is called once per frame
	void Update () {
        validSnapCount = 0;

        if(MotorPresentation.activeSelf)
        {
            if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_pres"))
            {
                motorAnim.SetBool("isBeginPresentation", false);
            }

            if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_placement"))
            {
                MotorPresentation.SetActive(false);
                MotorAssemblage.SetActive(true);
            }
        }
       

        if (snapDropZoneCount != 0)
        {
            foreach (GameObject sdz in allSnapDropZone)
            {
                if (sdz.transform.childCount == 2)
                {
                    validSnapCount += 1;
                }
            }

            if (validSnapCount == snapDropZoneCount && isFinished == false)
            {
                Debug.Log("GG !");
                isFinished = true;

                MotorPresentation.SetActive(true);
                MotorAssemblage.SetActive(false);
                MotorRunning();


            }
        }
        else
        {
            allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
            foreach (GameObject sdz in allSnapDropZone)
            {
                snapDropZoneCount += 1;
                snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;
            }
        }
        
	}

    //Activation de l'animation de la présentation des pièces du moteur
    public void MotorPresentationBegin()
    {
        motorAnim.SetBool("isBeginPresentation", true);
    }

    //Activation de l'animation du placement des pièces du moteur avant assemblage
    public void MotorMakeBegin()
    {
        motorAnim.SetBool("isBeginPlacement", true);
    }

    public void MotorRunning()
    {
        motorAnim.SetBool("isRunning", true);
    }
}
