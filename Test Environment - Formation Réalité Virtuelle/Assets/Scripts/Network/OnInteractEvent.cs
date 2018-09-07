using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRTK;

public class OnInteractEvent : NetworkBehaviour {

    public GameObject leftController;
    public GameObject rightController;

    private VRTK_InteractGrab leftInteractGrab;
    private VRTK_InteractGrab rightInteractGrab;

    private VRTK_InteractTouch leftTouch;
    private VRTK_InteractTouch rightTouch;

    private GameObject curLeftObjGrab;
    private GameObject curRightObjGrab;
    private GameObject curLeftObjTouch;
    private GameObject curRightObjTouch;

    private GameObject leftModel;
    private GameObject rightModel;

    private bool isGrabLeft = false;
    private bool isGrabRight = false;

    private bool isTouchLeft = false;
    private bool isTouchRight = false;

    private NetworkIdentity objNetId;



    // Use this for initialization
    void Start () {
        leftInteractGrab = leftController.GetComponent<VRTK_InteractGrab>();
        rightInteractGrab = rightController.GetComponent<VRTK_InteractGrab>();

        leftTouch = leftController.GetComponent<VRTK_InteractTouch>();
        rightTouch = rightController.GetComponent<VRTK_InteractTouch>();
    }
	
	// Update is called once per frame
	void Update () {

        if (isLocalPlayer)
        {
            GameObject leftObjectGrab = leftInteractGrab.GetGrabbedObject();
            GameObject rightObjectGrab = rightInteractGrab.GetGrabbedObject();

            GameObject leftObjectTouch = leftTouch.GetTouchedObject();
            GameObject rightObjectTouch = rightTouch.GetTouchedObject();

            //TOUCH EVENTS
            if (leftObjectTouch != null && isTouchLeft == false && leftObjectGrab == null)
            {
                isTouchLeft = true;
                curLeftObjTouch = leftObjectTouch;
                Debug.Log("Left Controller Touch : " + leftObjectTouch);
                CmdTouchGameObject(curLeftObjTouch);
            }

            if ((leftObjectTouch != curLeftObjTouch && isTouchLeft == true) || ( isTouchLeft == true && leftObjectGrab != null))
            {
                isTouchLeft = false;
                Debug.Log("Left Controller Untouch : " + leftObjectTouch);
                CmdUntouchGameObject(curLeftObjTouch);
                curLeftObjTouch = null;
            }

            if (rightObjectTouch != null && isTouchRight == false && rightObjectGrab == null)
            {
                isTouchRight = true;
                curRightObjTouch = rightObjectTouch;
                Debug.Log("right Controller Touch : " + rightObjectTouch);
                CmdTouchGameObject(curRightObjTouch);
            }

            if ((rightObjectTouch != curRightObjTouch && isTouchRight == true) || (isTouchRight == true && rightObjectGrab != null))
            {
                isTouchRight = false;
                Debug.Log("right Controller Untouch : " + curRightObjTouch);
                CmdUntouchGameObject(curRightObjTouch);
                curRightObjTouch = null;
            }



            //GRAB EVENTS
            if (leftObjectGrab != null && isGrabLeft == false)
            {
                isGrabLeft = true;
                curLeftObjGrab = leftObjectGrab;
                CmdGrabGameObject(curLeftObjGrab, 0);
            }
            else if(leftObjectGrab == null && isGrabLeft == true)
            {
                isGrabLeft = false;
                CmdUngrabGameObject(curLeftObjGrab, 0);
                curLeftObjGrab = null;
            }

            if (rightObjectGrab != null && isGrabRight == false)
            {
                isGrabRight = true;
                curRightObjGrab = rightObjectGrab;
                CmdGrabGameObject(curRightObjGrab, 1);
            }
            else if (rightObjectGrab == null && isGrabRight == true)
            {
                isGrabRight = false;
                CmdUngrabGameObject(curRightObjGrab, 1);
                curRightObjGrab = null;
            }

            
        }
    }

    [Command]
    void CmdTouchGameObject(GameObject go) //int leftOrRight : Left = 0 and Right = 1
    {
        if(go.GetComponent<NetworkIdentity>() == null)
        {
            return;
        }
        // get the object's network ID
        objNetId = go.GetComponent<NetworkIdentity>();

        // use a Client RPC function to "change" the object on all clients
        RpcTouchGameObject(go);
    }

    [Command]
    void CmdUntouchGameObject(GameObject go) //int leftOrRight : Left = 0 and Right = 1
    {
        if (go.GetComponent<NetworkIdentity>() == null)
        {
            return;
        }
        // get the object's network ID
        objNetId = go.GetComponent<NetworkIdentity>();

        // use a Client RPC function to "change" the object on all clients
        RpcUntouchGameObject(go);
    }

    [Command]
    void CmdGrabGameObject(GameObject go, int leftOrRight) //int leftOrRight : Left = 0 and Right = 1
    {
        if (go.GetComponent<NetworkIdentity>() == null)
        {
            return;
        }
        // get the object's network ID
        objNetId = go.GetComponent<NetworkIdentity>();

        // use a Client RPC function to "change" the object on all clients
        RpcGrabGameObject(go, leftOrRight);
    }

    [Command]
    void CmdUngrabGameObject(GameObject go, int leftOrRight) //int leftOrRight : Left = 0 and Right = 1
    {
        if(go == null)
        {
            return;
        }

        if (go.GetComponent<NetworkIdentity>() == null)
        {
            return;
        }
        // get the object's network ID
        objNetId = go.GetComponent<NetworkIdentity>();

        // use a Client RPC function to "change" the object on all clients
        RpcUngrabGameObject(go, leftOrRight);

    }

    [ClientRpc]
    void RpcTouchGameObject(GameObject go)
    {
        if(!isLocalPlayer)
        {
            go.GetComponent<VRTK_InteractableObject>().ToggleHighlight(true);
        }
    }

    [ClientRpc]
    void RpcUntouchGameObject(GameObject go)
    {
        if (!isLocalPlayer)
        {
            go.GetComponent<VRTK_InteractableObject>().ToggleHighlight(false);
        }
    }

    [ClientRpc]
    void RpcGrabGameObject(GameObject go, int leftOrRight)
    {
        if(!isLocalPlayer)
        {
            
            if (!go.GetComponent<Joint>())
            {
                if(leftOrRight == 0)
                {
                    if (leftModel == null)
                    {
                        leftModel = GameObject.FindGameObjectWithTag("leftControllerModel");
                    }

                    leftModel.SetActive(false);
                }

                if (leftOrRight == 1)
                {
                    if (rightModel == null)
                    {
                        rightModel = GameObject.FindGameObjectWithTag("rightControllerModel");
                    }

                    rightModel.SetActive(false);
                }

                Destroy(go.GetComponent<Rigidbody>());
            }
            else
            {
                Debug.Log("(No RigidBody to remove) A joint is active on this Object : " + go.GetComponent<Joint>());
            }
        }
        
    }

    [ClientRpc]
    void RpcUngrabGameObject(GameObject go, int leftOrRight)
    {
        if (!isLocalPlayer)
        {
            if (!go.GetComponent<Rigidbody>())
            {
                if (leftOrRight == 0)
                {
                    leftModel.SetActive(true);
                }
                else if (leftOrRight == 1)
                {
                    rightModel.SetActive(true);
                }
                //Suppression temporaire : Bug avec le SnapDropZone
                //go.AddComponent<Rigidbody>();
            }
            
        }
    }
}
