using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Titre : Pause Menu Script (TODO::Changer le nom du script)
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     script contenant :
///         - Une fonction servant à reset / reload la scène en cours
///         - une fonction servant à changer de scène quand celui-ci est appelé
///     
/// </summary>

public class PauseMenuScript : MonoBehaviour {

    public SteamVR_LoadLevel loadLevel;

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }


    public void ChangeScene(string sceneName)
    {
        ///Default LoadLevel///
        //SteamVR_LoadLevel.Begin(sceneName); 

        ///Custom LoadLevel///
        loadLevel.levelName = sceneName;
        loadLevel.Trigger();
    }

}
