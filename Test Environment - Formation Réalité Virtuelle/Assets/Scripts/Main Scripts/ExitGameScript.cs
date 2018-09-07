using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//// <summary>
/// Titre : Exit Game Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///    Script permettant de fermer l'application quand ExitGame() est appelé
///     
/// </summary>

public class ExitGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitGame()
    {
        Application.Quit();
    }
}
