using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorielGameManager : MonoBehaviour {

    [Header("Tutorial Game Settings")]

    public string snapDropTag;
    public GameObject nextStepZone;
    private GameObject[] allSnapDropZone;

    private int snapDropZoneCount;
    private int validSnapCount;
    private bool isFinished;


    // Use this for initialization
    void Start()
    {
        snapDropZoneCount = 0;
        validSnapCount = 0;
        isFinished = false;

        allSnapDropZone = GameObject.FindGameObjectsWithTag(snapDropTag);
        snapDropZoneCount = GameObject.FindGameObjectsWithTag(snapDropTag).Length;
    }

    // Update is called once per frame
    void Update()
    {
        if(isFinished == false)
        {
            if (snapDropZoneCount != 0)
            {

                if (validSnapCount == snapDropZoneCount && isFinished == false)
                {
                    Debug.Log("GG !");
                    isFinished = true;
                    nextStepZone.SetActive(true);
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

    public void addOneCompletedSpot()
    {
        validSnapCount += 1;
    }

    public void supprOneCompletedSpot()
    {
        validSnapCount -= 1;
    }

}
