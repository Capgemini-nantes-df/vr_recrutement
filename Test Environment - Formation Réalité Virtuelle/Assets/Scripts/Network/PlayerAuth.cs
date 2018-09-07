using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAuth : NetworkBehaviour {

    [Command]
    public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
    {
        Debug.Log("giving player auth now...");
        var iObject = NetworkServer.FindLocalObject(objectId);
        var networkIdentity = iObject.GetComponent<NetworkIdentity>();
        var otherOwner = networkIdentity.clientAuthorityOwner;

        if (otherOwner == player.connectionToClient)
        {
            Debug.Log("player already has auth");
            //return;
        }
        else
        {
            if (otherOwner != null)
            {
                networkIdentity.RemoveClientAuthority(otherOwner);
            }
            Debug.Log("player now has auth!");
            networkIdentity.localPlayerAuthority = true;
            networkIdentity.AssignClientAuthority(player.connectionToClient);
        }
    }
}
