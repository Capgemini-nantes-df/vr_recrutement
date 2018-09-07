using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSnapDropOptions : NetworkBehaviour {

    public bool initialAppearanceSnapDrop;

    private GameObject childObj;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (initialAppearanceSnapDrop == true)
        {
            if (transform.childCount == 2)
            {
                if(isServer)
                {
                    CmdInitAppearanceObj(this.transform.GetChild(1).gameObject);
                } 
            }
        }
    }

    [Command]
    void CmdInitAppearanceObj(GameObject go)
    {
        RpcInitAppearanceObj(go);
    }

    //Obj Snap take initial appearance of the SnapDropZone
    [ClientRpc]
    void RpcInitAppearanceObj(GameObject go)
    {
        childObj = go;

        if (childObj.GetComponent<MeshRenderer>())
        {
            childObj.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            Renderer[] rs = childObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
                r.enabled = false;

        }

        childObj.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = true;
    }
}
