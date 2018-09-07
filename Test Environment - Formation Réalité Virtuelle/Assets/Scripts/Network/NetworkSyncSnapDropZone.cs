using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VRTK;

public class NetworkSyncSnapDropZone : NetworkBehaviour {

    public GameObject highlightObj;

    private bool isAuth = false;

    private bool isActive;

    private void Start()
    {
        isActive = highlightObj.activeSelf;
    }

    private void Update()
    {
        if (isServer)
        {
            if (isAuth == false)
            {
                GiveAuthToPlayer();
            }

            if (isAuth == true)
            {
                if (isActive != highlightObj.activeSelf)
                {
                    isActive = !isActive;
                    IsHighlighted(isActive);
                }
            }   
        }
    }

    public void IsHighlighted(bool isInSnapZone)
    {
        if (isServer)
        {
            if (isInSnapZone == true)
            {
                CmdHighlightObject();
            }
            else
            {
                CmdUnhighlightObject();
            }
        }
    }

    [Command]
    void CmdHighlightObject()
    {
        RpcHighlightObject();
    }

    [Command]
    void CmdUnhighlightObject()
    {
        RpcUnhighlightObject();
    }

    [ClientRpc]
    void RpcHighlightObject()
    {
        if (!isServer)
        {
            highlightObj.SetActive(true);
        }
    }

    [ClientRpc]
    void RpcUnhighlightObject()
    {
        if (!isServer)
        {
            highlightObj.SetActive(false);
        }
    }

    public void GiveAuthToPlayer()
    {
        Debug.Log("need to give player auth to " + gameObject + "...");

        GameObject player = null;

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (pl.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                player = pl;
            }
        }
        if (player != null)
        {
            player.GetComponent<PlayerAuth>().CmdSetAuth(netId, player.GetComponent<NetworkIdentity>());
            isAuth = true;
        }
        else
        {
            Debug.Log("No player Local Found...");
        }
    }
}
