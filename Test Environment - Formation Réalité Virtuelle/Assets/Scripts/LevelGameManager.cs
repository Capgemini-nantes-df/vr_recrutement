using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameManager : MonoBehaviour {

    [Header("Motor Animation Settings")]

    public Animator motorAnim;

    public GameObject MotorPresentation;
    public GameObject MotorAssemblage;

    [Header("Lvl Game Settings")]

    public GameObject SteamVR_SDK;
    public GameObject goText;
    public string snapDropTag;
    public GameObject buttonStart;
    public GameObject buttonEnd;

    [Header("Tablet Score Settings")]
    public GameObject panelScore;
    public TextMeshPro timerText;
    public TextMeshPro scoringText;
    public TextMeshPro debugSnapText;

    [Header("Music Settings")]
    public AudioClip musicSound;
    public string musicDiffuseTag;
    public AudioSource victorySound;

    private GameObject[] allSnapDropZone;
    private GameObject[] allMusicDiffusers;

    private int snapDropZoneCount;
    private int validSnapCount;
    private bool isFinished;

    private string lastInteractObjName;

    private float startTime;


    // Use this for initialization
    void Start () {
        snapDropZoneCount = 0;
        isFinished = false;
        validSnapCount = 0;

        allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
        snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;

        allMusicDiffusers = GameObject.FindGameObjectsWithTag(musicDiffuseTag);

        foreach (GameObject md in allMusicDiffusers)
        {
        md.GetComponent<AudioSource>().clip = musicSound;
        md.GetComponent<AudioSource>().Play();
        }

    }

    // Update is called once per frame
    void Update () {

        //Sync of all Music Diffusers
        for (int i=1; i < allMusicDiffusers.Length; i++ )
        {
            allMusicDiffusers[i].GetComponent<AudioSource>().timeSamples = allMusicDiffusers[0].GetComponent<AudioSource>().timeSamples;
        }

        if (MotorPresentation.activeSelf)
        {
            if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_pres"))
            {
                motorAnim.SetBool("isBeginPresentation", false);
            }

            if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_placement"))
            {
                buttonStart.SetActive(false);
                MotorPresentation.SetActive(false);
                MotorAssemblage.SetActive(true);
                goText.SetActive(true);
                panelScore.SetActive(true);
                SteamVR_SDK.transform.position = new Vector3(SteamVR_SDK.transform.position.x, SteamVR_SDK.transform.position.y, SteamVR_SDK.transform.position.z +1.373f);
                startTime = Time.time;
            }
        }
       
        if(isFinished == false)
        {
            if (snapDropZoneCount != 0)
            {
                UpdateTimeText();

                if (validSnapCount == snapDropZoneCount)
                {
                    UpdateDebugSnapText("Félicitation !");
                    isFinished = true;

                    victorySound.Play();

                    buttonEnd.SetActive(true);
                    MotorPresentation.SetActive(true);
                    MotorAssemblage.SetActive(false);
                    MotorRunning();
                }
            }
            else
            {
                allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
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

    /*public void addOneCompletedSpot(string name)
    {
        lastInteractObjName = name;
        Debug.Log("Une pièce a été placée");
        validSnapCount += 1;
        Debug.Log("Pièce placée : " + validSnapCount + " sur " + snapDropZoneCount);
        UpdateScoringText();
        UpdateDebugSnapText(true);
    }*/

    /*public void supprOneCompletedSpot(string name)
    {
        lastInteractObjName = name;
        Debug.Log("une pièce a été retirée");
        validSnapCount -= 1;
        Debug.Log("Pièce retirée : " + validSnapCount );
        UpdateScoringText();
        UpdateDebugSnapText(false);
    }*/

    public void UpdateNumberCompletedSpot(string name)
    {
        int currentValidSnapCount = validSnapCount;
        validSnapCount = 0;
        lastInteractObjName = name;

        foreach (GameObject sdz in allSnapDropZone)
        {
            if(sdz.transform.childCount == 2)
            {
                validSnapCount += 1;
            }
        }

        if(validSnapCount < currentValidSnapCount)
        {
            Debug.Log("Une pièce a été placée");
            Debug.Log("Pièces retirée : " + validSnapCount + " sur " + snapDropZoneCount);
            UpdateScoringText();
            UpdateDebugSnapText("La pièce : " + lastInteractObjName + " a été retiré.");
        }
        else if(validSnapCount > currentValidSnapCount)
        {
            Debug.Log("une pièce a été retirée");
            Debug.Log("Pièce placée : " + validSnapCount + " sur " + snapDropZoneCount);
            UpdateScoringText();
            UpdateDebugSnapText("La pièce : " + lastInteractObjName + " a été placé.");
        }
        
    }

    private void UpdateScoringText()
    {
        scoringText.text = validSnapCount + "/" + snapDropZoneCount;
    }

    private void UpdateDebugSnapText(string textUpdate)
    {
        debugSnapText.text = textUpdate;

    }

    private void UpdateTimeText()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        if( minutes.Length == 1)
        {
            minutes = "0" + minutes;
        }
        string seconds = (t % 60).ToString("f2");
        if( seconds.Length == 4)
        {
            seconds = "0" + seconds;
        }

        timerText.text = minutes + ":" + seconds;
    }
}
