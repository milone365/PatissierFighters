using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using M;

public class NetWorkCleaner : MonoBehaviour
{
   public static NetWorkCleaner instance;
   

    private void Awake()
    {
        instance = this;
    }

    public void removeObjectFromServer(GameObject g)
    {
        NetworkServer.Destroy(g);
    }
}


   
