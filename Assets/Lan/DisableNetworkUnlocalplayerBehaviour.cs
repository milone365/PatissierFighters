using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DisableNetworkUnlocalplayerBehaviour : NetworkBehaviour
{
    
    Behaviour[] behaviours;
    // Start is called before the first frame update
    void Start()
    {
        behaviours = GetComponentsInChildren<Behaviour>();
        if(!isLocalPlayer)
        {
            foreach(var behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }
    }
    /*
    private void OnApplicationFocus(bool focusStatus)
    {
        if(isLocalPlayer)
        {
            foreach(var behaviour in behaviours)
            {
                behaviour.enabled = focusStatus;
            }
        }
    }
    */
}
