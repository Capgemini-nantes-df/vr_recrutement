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

}
