using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Titre : All Snap Highlight Helper Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Highlight des Zone de drop restants dont l'objet associé n'est pas encore assemblé sur le moteur
///     A placer en component du GameObjet du système de la scène
/// 
///     
/// </summary>

public class AllSnapHighlightHelperScript : MonoBehaviour {

    private bool currentState;

    //On met la liste des Gameobjects consernés
    [Tooltip("A list of identifiers to check for against the given check type (either tag or script).")]
    public List<GameObject> highlightObjects = new List<GameObject>();

    private GameObject obj;


    // Use this for initialization
    void Start () {
        //L'état du highlightAll est false au démarage
        currentState = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    //Function d'highlight des objets - A appeler en Event
    public void HighlightAllDropZone()
    {
        currentState = !currentState;

        for (int i = 0; i < highlightObjects.Count; i++)
        {
            obj = highlightObjects[i];
            obj.GetComponent<VRTK_SnapDropZone>().highlightAlwaysActive = currentState;

            //On vérifie si le gameobjet est déjà placé ou non sur le moteur
            if (obj.transform.childCount == 1)
            { 
                //si celui-ci n'est pas placé alors on active/desactive le highlight en fonction de l'état
                obj.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(currentState);
                //Debug.Log(obj + " is " + currentState);
            }
        }

        Debug.Log("All highlightDropZone are " + currentState);
    }
}
