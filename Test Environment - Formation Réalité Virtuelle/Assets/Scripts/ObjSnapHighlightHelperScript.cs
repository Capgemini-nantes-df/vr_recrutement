using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Highlighters;

public class ObjSnapHighlightHelperScript : MonoBehaviour {

    public GameObject SnapDropZone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<VRTK_MaterialColorSwapHighlighter>() != null && SnapDropZone.transform.childCount == 1)
        {
            HighlightDropZoneOn();
        }
        else
        {
            HighlightDropZoneOff();
        }
	}

    public void HighlightDropZoneOn()
    {
        SnapDropZone.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = true;
        SnapDropZone.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
    }

    public void HighlightDropZoneOff()
    {
        SnapDropZone.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = false;
        SnapDropZone.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
    }
}
