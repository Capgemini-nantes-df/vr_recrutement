using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.Networking;

public class NetworkSyncSystemManager : NetworkBehaviour {

    public Animator capsuleAnimation;

    private int score = 0;
    private bool isAuth = false;

    public void UpdateScore(bool isSnap)
    {
        if (isServer)
        {
            if (isAuth == false)
            {
                GiveAuthToPlayer();
            }
            
            if (isAuth == true)
            {
                CmdUpdateScore(gameObject, isSnap);
            }
        }
    }

    public void Update()
    {
        if(score == 2)
        {
            capsuleAnimation.SetBool("isActive", true);
        }
    }

    [Command]
    void CmdUpdateScore(GameObject go, bool isSnap)
    {
        RpcUpdateScore(go, isSnap);
    }

    [ClientRpc]
    void RpcUpdateScore(GameObject go, bool isSnap)
    {
        if(isSnap == true)
        {
            score += 1;
            Debug.Log(go + " has been snapped !");
            Debug.Log("Score Update : " + score + " pt(s)");
        }
        else
        {
            score -= 1;
            Debug.Log(go + " has been unsnapped !");
            Debug.Log("Score Update : " + score + " pt(s)");
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
