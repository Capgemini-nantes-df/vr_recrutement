using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class LevelGameManager : MonoBehaviour {

    [Header("Motor Animation Settings")]
    public Animator motorAnim;

    public GameObject MotorPresentation;
    public GameObject MotorAssemblage;

    [Header("Lvl Game Settings")]
    public GameObject SteamVR_SDK;
    public GameObject goText;
    public GameObject tvHelpPanel;
    public string snapDropTag;
    public GameObject buttonStart;


    [Header("End Game Settings")]
    public GameObject tabletScore;
    public Transform endTransformTablet;
    public GameObject tvEndPanel;
    public GameObject buttonEnd;

    [Header("Respawns Settings")]
    public GameObject toSnap;
    public SpawnerBox spawnerBox;

    [Header("Tablet Score Settings")]
    public GameObject panelScore;
    public TextMeshPro timerText;
    public TextMeshPro scoringText;
    public TextMeshPro debugSnapText;

    [Header("Music Settings")]
    public AudioClip musicSound;
    public string musicDiffuseTag;
    public AudioSource victorySound;

    [Header("Controllers Settings")]
    public VRTK_InteractGrab leftController;
    public VRTK_InteractGrab rightController;

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
                tvHelpPanel.SetActive(true);
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

                    //buttonEnd.SetActive(true);

                    MotorPresentation.SetActive(true);
                    MotorAssemblage.SetActive(false);

                    //Run Motor running Animation 
                    MotorRunning();

                    ///Enable TV End Panel///
                    for (int i = 0; i < tvEndPanel.transform.parent.transform.childCount; i++)
                    {
                        if (tvEndPanel.transform.parent.transform.GetChild(i).gameObject.activeSelf == true)
                        {
                            tvEndPanel.transform.parent.transform.GetChild(i).gameObject.SetActive(false);
                        }
                    }
                    tvEndPanel.SetActive(true);

                    ///change position of tablet Score
                    leftController.ForceRelease(false);
                    rightController.ForceRelease(false);
                    tabletScore.transform.position = endTransformTablet.position;
                    tabletScore.transform.rotation = endTransformTablet.rotation;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RespawnAllObjNotSnapped();
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

    public void RespawnAllObjNotSnapped()
    {
        foreach(RespawnObject child in toSnap.GetComponentsInChildren<RespawnObject>())
        {
            child.RespawnToStart();
        }

        spawnerBox.RespawnPistonsOutOfTheBox();
    }
}
