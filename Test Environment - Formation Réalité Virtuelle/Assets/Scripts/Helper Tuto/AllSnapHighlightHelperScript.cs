using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class AllSnapHighlightHelperScript : MonoBehaviour {

    private bool currentState;

    [Tooltip("A list of identifiers to check for against the given check type (either tag or script).")]
    public List<GameObject> highlightObjects = new List<GameObject>();

    private GameObject obj;


    // Use this for initialization
    void Start () {
        currentState = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void HighlightAllDropZone()
    {
        currentState = !currentState;

        for (int i = 0; i < highlightObjects.Count; i++)
        {
            obj = highlightObjects[i];
            obj.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = currentState;

            if (obj.transform.childCount == 1)
            { 
            obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(currentState);
            //Debug.Log(obj + " is " + currentState);
            }
        }

        Debug.Log("All highlightDropZone are " + currentState);
    }
}
