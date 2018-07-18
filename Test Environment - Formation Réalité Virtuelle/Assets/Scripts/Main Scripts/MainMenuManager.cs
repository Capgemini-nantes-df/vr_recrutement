using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelectPanel(GameObject panel)
    {
        GameObject parent = panel.transform.parent.gameObject;
        int childs = parent.transform.childCount;
        int i = 0;

        panel.SetActive(true);
        
        while(i < childs)
        {
            if(parent.transform.GetChild(i).gameObject != panel)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
            }
            i++;
        }
    }


}
