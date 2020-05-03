using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyNetworkManager : NetworkBehaviour
{
    Mymanager manager;
    private void Start()
    {

        if (manager != null)
        {
            manager.StopHost();
            manager.client.Disconnect();

        }
        else
        {
            Debug.Log("not find networkmanager");
        }
    }


    [Command]
    void CmdDestroylan()
    {
        destroyLan();
        RpcDestroylan();
    }
    [ClientRpc]
    void RpcDestroylan()
    {
        if (!isServer)
        {
            destroyLan();
        }
    }
    public void destroyLan()
    {
        manager = FindObjectOfType<Mymanager>();
        if (manager != null)
        {
            Destroy(manager.gameObject);
        }
    }
}
