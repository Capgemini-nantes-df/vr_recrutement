using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
