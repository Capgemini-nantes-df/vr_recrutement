using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameManager : MonoBehaviour {

    [Header("Motor Animation Settings")]

    public Animator motorAnim;

    public GameObject MotorPresentation;
    public GameObject MotorAssemblage;

    [Header("Lvl Game Settings")]

    public string snapDropTag;
    public GameObject scoringText;

    private GameObject[] allSnapDropZone;

    private int snapDropZoneCount;
    private int validSnapCount;
    private bool isFinished;


    // Use this for initialization
    void Start () {
        snapDropZoneCount = 0;
        isFinished = false;
        validSnapCount = 0;


        allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
        snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;
    }
	
	// Update is called once per frame
	void Update () {

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
       
        if(isFinished == false)
        {
            if (snapDropZoneCount != 0)
            {
                
                if (validSnapCount == snapDropZoneCount)
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

    public void addOneCompletedSpot()
    {
        Debug.Log("Une pièce a été placée");
        validSnapCount += 1;
        Debug.Log("Pièces placées : " + validSnapCount + " sur " + snapDropZoneCount);
        UpdateScoringText();
    }

    public void supprOneCompletedSpot()
    {
        Debug.Log("une pièce a été retirée");
        validSnapCount -= 1;
        Debug.Log("Pièces placées : " + validSnapCount + " sur " + snapDropZoneCount);
        UpdateScoringText();
    }

    public void UpdateScoringText()
    {
        scoringText.GetComponent<Text>().text = validSnapCount + "/" + snapDropZoneCount;
    }
}
