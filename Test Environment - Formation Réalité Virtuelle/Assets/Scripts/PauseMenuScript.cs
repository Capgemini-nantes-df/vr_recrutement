using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {

    public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene);
        SceneManager.LoadScene(scene.name);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ChangeToSceneTuto()
    {
        SceneManager.LoadScene("Tutoriel_TestZone1", LoadSceneMode.Single);
    }

    public void ChangeToScene1()
    {
        SceneManager.LoadScene("Lvl1_Engine_1", LoadSceneMode.Single);
    }

    public void ChangeToScene2()
    {
        SceneManager.LoadScene("Lvl2_Engine_2", LoadSceneMode.Single);
    }

}
