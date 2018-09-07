using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRTK;

public class PlayerSetup : NetworkBehaviour {

    [Header("Global VR Settings")]
    [SerializeField]
    VRTK_SDKManager sdkManager;
    [SerializeField]
    GameObject VRTK_Scripts;
    [SerializeField]
    GameObject playerVR;
    [SerializeField] 
    OnInteractEvent onInteractEvent;
    [SerializeField]
    Behaviour[] componentsVRToDisable;
    [SerializeField]
    Behaviour[] componentsVRTransformChild;


    [Header("Controller VR Settings")]
    public GameObject controllerLeft;
    public GameObject controllerModelLeft;
    public GameObject controllerRight;
    public GameObject controllerModelRight;


    [Header("global Classic Settings")]
    [SerializeField]
    GameObject playerClassic;
    [SerializeField]
    PlayerClassicMoves playerClassicMoves;
    [SerializeField]
    Behaviour[] componentsClassicToDisable; 
 
    private Camera sceneCamera;

    // Use this for initialization
    void Start () {
        NetworkInstanceId gamePlayerNetID = gameObject.GetComponent<NetworkIdentity>().netId;
        Debug.Log("ID player : " + gamePlayerNetID);

        if (isServer)
        {
            if (!isLocalPlayer)
            {
                Debug.Log("Attempt Player Classic Online init...");
                LoadClassicOnlinePlayer();
                Debug.Log("New Player Online !");
            }
            else
            {
                Debug.Log("Attempt Player VR Local Host init...");
                LoadVRLocalPlayer();
                Debug.Log("You are now Online !");
            }
        }
        else
        {
            if (!isLocalPlayer)
            {
                int i = 0;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (int.Parse(gamePlayerNetID.ToString()) > int.Parse(go.GetComponent<NetworkIdentity>().netId.ToString()))
                    {
                        i++;
                    }
                }

                if (i == 0)
                {
                    Debug.Log("Attempt Player VR Online Client init...");
                    LoadlVROnlinePlayer();
                    Debug.Log("New Player Online !");
                }
                else
                {
                    Debug.Log("Attempt Player Classic Online init...");
                    LoadClassicOnlinePlayer();
                    Debug.Log("New Player Online !");
                }   
            }
            else
            {
                Debug.Log("Player Classic Local Client init...");
                LoadClassicLocalPlayer();
                Debug.Log("You are now Online !");
            }
        }


    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    void LoadClassicLocalPlayer()
    {
        sceneCamera = Camera.main;
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(false);
        }

        for (int i = 0; i < componentsVRTransformChild.Length; i++)
        {
            componentsVRTransformChild[i].enabled = false;
        }

        Destroy(playerVR);
        Destroy(onInteractEvent);
        Destroy(VRTK_Scripts);
        Destroy(sdkManager);
        playerClassic.SetActive(true);
    }

    void LoadClassicOnlinePlayer()
    {
        for (int i = 0; i < componentsClassicToDisable.Length; i++)
        {
            componentsClassicToDisable[i].enabled = false;
        }
        Destroy(VRTK_Scripts);
        Destroy(onInteractEvent);
        Destroy(playerVR);
        Destroy(sdkManager);
        playerClassic.SetActive(true); ;
    }

    void LoadVRLocalPlayer()
    {
        VRTK_Scripts.SetActive(true);
        sdkManager.enabled = true;
        Destroy(controllerModelLeft);
        Destroy(controllerModelRight);
        Destroy(playerClassicMoves);
        Destroy(playerClassic);
    }

    void LoadlVROnlinePlayer()
    {
        for (int i = 0; i < componentsVRToDisable.Length; i++)
        {
            componentsVRToDisable[i].enabled = false;
        }

        controllerLeft.SetActive(true);
        controllerRight.SetActive(true);
        Destroy(playerClassicMoves);
        Destroy(playerClassic);
        Destroy(VRTK_Scripts);
        Destroy(sdkManager);
        playerVR.SetActive(true);
    }

    private void OnDisable()
    {
    if (sceneCamera != null)
        {
        sceneCamera.gameObject.SetActive(true);
        }
    }

}
