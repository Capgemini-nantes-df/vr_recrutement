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

    public void ChangeToScene4()
    {
        SceneManager.LoadScene("testZone4", LoadSceneMode.Single);
    }

    public void ChangeToScene5()
    {
        SceneManager.LoadScene("testZone5", LoadSceneMode.Single);
    }

}
