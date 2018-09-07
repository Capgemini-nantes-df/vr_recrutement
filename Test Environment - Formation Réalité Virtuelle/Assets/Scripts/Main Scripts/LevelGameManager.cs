using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

/// <summary>
/// Titre : Level Game Manager
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Game Manager d'une scène standard d'un niveau
/// 
/// </summary>

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

        //On calcule le nombre de zone de drop (objet à placer) dans la scène (pour le décompte des points)
        allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
        snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;

        //On recherche les diffuseurs de musiques et on la lance
        allMusicDiffusers = GameObject.FindGameObjectsWithTag(musicDiffuseTag);

        foreach (GameObject md in allMusicDiffusers)
        {
        md.GetComponent<AudioSource>().clip = musicSound;
        md.GetComponent<AudioSource>().Play();
        }

    }

    // Update is called once per frame
    void Update () {

        //Syncde tout les Music Diffusers
        for (int i=1; i < allMusicDiffusers.Length; i++ )
        {
            allMusicDiffusers[i].GetComponent<AudioSource>().timeSamples = allMusicDiffusers[0].GetComponent<AudioSource>().timeSamples;
        }

        //Action si le gameObjet MotorPresentation est activé
        if (MotorPresentation.activeSelf)
        {
            //Action si l'animation "Motor_end_pres" de motorAnim est lancé
            if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_pres"))
            {
                motorAnim.SetBool("isBeginPresentation", false);
            }

            //Action si l'animation "motor_end" de motorAnim est lancé
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
       
        //action si la tâche est accompli (toutes les pièces ont été assemblées)
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

                    MotorPresentation.SetActive(true);
                    MotorAssemblage.SetActive(false);

                    //Lance l'animation MotorRunning
                    MotorRunning();

                    //Active le panel de fin de niveau
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

                //respawn tout les objets pas encore placés si l'on presse la touche "Space"
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    RespawnAllObjNotSnapped();
                }
            }

            //Si aucunes zone de drop n'est détecté, on refait une tentative de recherche des zones de drop
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

    //activation de l'animation du moteur en mouvement
    public void MotorRunning()
    {
        motorAnim.SetBool("isRunning", true);
    }

    //Fonction permettant d'effectuer un update du nombre d'objets restants à placer
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

    //fonction permettant d'update le score dans le champs texte
    private void UpdateScoringText()
    {
        scoringText.text = validSnapCount + "/" + snapDropZoneCount;
    }

    //fonction permettant d'update le texte de l'action effectuer (placer ou retirer un objet)
    private void UpdateDebugSnapText(string textUpdate)
    {
        debugSnapText.text = textUpdate;

    }

    //fonction permettant l'update du timer de la tâche
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

    //Fonction permettant de respawn tout les objets pas encore placer (pas encore dans leur snap zone)
    public void RespawnAllObjNotSnapped()
    {
        foreach(RespawnObject child in toSnap.GetComponentsInChildren<RespawnObject>())
        {
            child.RespawnToStart();
        }

        spawnerBox.RespawnPistonsOutOfTheBox();
    }
}
