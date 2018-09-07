using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Titre : Main Menu Manager
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Game Manager de la scène du menu Principal
///     
/// </summary>

public class MainMenuManager : MonoBehaviour {

    //Fonction de selection de Panel
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
