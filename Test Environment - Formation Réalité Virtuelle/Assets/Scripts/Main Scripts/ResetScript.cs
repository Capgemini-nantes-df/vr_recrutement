using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour {

    /// <summary>
    /// Titre : ResetScript
    /// Auteur : GOISLOT Renaud
    /// Description :
    /// 
    ///     DEPRECATED ON THIS PROJET
    ///     
    ///     Script permettant de reset la scene si la touche désigné est pressé. (Script pour debug)
    ///     
    /// </summary>

    [Header("Reset key Settings")]
    public string resetKey;
	
	// Update is called once per frame
	void Update () {

        //reset de la scene en fonction de la touche choisi
        if (Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), resetKey)))
        {
            Scene scene = SceneManager.GetActiveScene();
            Debug.Log(scene);
            SceneManager.LoadScene(scene.name);
        }
       
    }
}
