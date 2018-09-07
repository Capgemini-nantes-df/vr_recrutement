using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraVision : MonoBehaviour {

    private Camera viveCamera;
    private Camera desktopCamera;

    private bool isFullScreen;

	// Use this for initialization
	void Start () {
        isFullScreen = false;
        initCameras();
    }

    // Update is called once per frame
    void Update()
    {
        if(viveCamera == null || desktopCamera == null)
        {
            initCameras();
            return;
        }

        if (Input.GetKeyDown("space"))
        {
            if (isFullScreen == false)
            {
                desktopCamera.enabled = false;
                viveCamera.rect = new Rect(0, 0, 1f, 1f);
                isFullScreen = true;
            }
            else
            {
                desktopCamera.enabled = true;
                viveCamera.rect = new Rect(0, 0, 0.4f, 0.4f);
                isFullScreen = false;
            }
        }
    } 
    
    void initCameras()
    {
        if(GameObject.FindWithTag("MainCamera"))
        {
            desktopCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        if(GameObject.FindWithTag("2ndViveCamera"))
        {
            viveCamera = GameObject.FindWithTag("2ndViveCamera").GetComponent<Camera>();
        }
        
    }
}
