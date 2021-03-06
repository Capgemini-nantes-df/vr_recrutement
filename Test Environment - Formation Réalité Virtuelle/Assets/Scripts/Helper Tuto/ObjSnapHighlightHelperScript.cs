﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;

/// <summary>
/// Titre : Obj Snap Highlight Helper Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     DEPRECATED ON THIS PROJECT
///     (Préférer utiliser AllSnapHighlightHelperScript)
///     
///     Highlight des Zone de drop restants dont l'objet associé n'est pas encore assemblé sur le moteur
///     A placer en component du GameObjet du système de la scène
///     
/// </summary>

public class ObjSnapHighlightHelperScript : MonoBehaviour {

    public GameObject[] SnapDropZones;

    private bool isOn;

    // Use this for initialization
    void Start () {
        isOn = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<VRTK_MaterialColorSwapHighlighter>() != null)
        {
            if (isOn == false)
            {
                foreach (GameObject sdz in SnapDropZones)
                {
                    if (sdz.transform.childCount == 1)
                    {
                        HighlightDropZoneOn(sdz);
                    }
                }
                isOn = true;
            }
            
        }
        else if(isOn == true)
        {
            foreach (GameObject sdz in SnapDropZones)
            {
                HighlightDropZoneOff(sdz);
            }
            isOn = false;
        }
    }

    public void HighlightDropZoneOn(GameObject SnapDropZone)
    {
        SnapDropZone.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = true;
        SnapDropZone.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HighlightDropZoneOff(GameObject SnapDropZone)
    {
        SnapDropZone.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = false;
        SnapDropZone.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
}
