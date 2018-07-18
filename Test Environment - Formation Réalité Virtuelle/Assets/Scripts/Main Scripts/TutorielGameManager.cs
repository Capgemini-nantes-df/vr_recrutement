using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorielGameManager : MonoBehaviour {

    [Header("Tutorial Game Settings")]

    public string snapDropTag;
    public GameObject nextStepZone;
    private GameObject[] allSnapDropZone;

    public int snapDropZoneCount;

    private int validSnapCount;
    private bool isFinished;


    // Use this for initialization
    void Start()
    {
        validSnapCount = 0;
        isFinished = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinished == false)
        {
            if (validSnapCount == snapDropZoneCount)
            {
                Debug.Log("GG !");
                isFinished = true;
                nextStepZone.SetActive(true);
            }
            
        }
       

    }

    public void addOneCompletedSpot()
    {
        Debug.Log("Une pièce a été placée");
        validSnapCount += 1;
        Debug.Log("Pièces placées : " + validSnapCount + " sur " + snapDropZoneCount);
    }

    public void supprOneCompletedSpot()
    {
        Debug.Log("une pièce a été retirée");
        validSnapCount -= 1;
        Debug.Log("Pièces placées : " + validSnapCount + " sur " + snapDropZoneCount);
    }

}
