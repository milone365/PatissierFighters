using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class M_GameManager :NetworkBehaviour
{
    private void Start()
    {
        Invoke("CmdStart", 10);
    }
    [ClientRpc]
    void RpcStart()
    {
        if (!isServer)
        {
            M_Clock clock = FindObjectOfType<M_Clock>();
            clock.CmdStartgame();
        }
    }
    [Command]
    void CmdStart()
    {
        M_Clock clock = FindObjectOfType<M_Clock>();
        clock.CmdStartgame();
    }
    public void EndGame()
    {
        CmdEndGame();
    }
    [Command]
    void CmdEndGame()
    {
        GameObject fd = GameObject.Find("FadeScreen");
        fd.GetComponent<Animation>().Play("fade");
        RpcEndGame();
    }
    [ClientRpc]
    void RpcEndGame()
    {
        if (!isServer)
        {
            GameObject fd = GameObject.Find("FadeScreen");
            fd.GetComponent<Animation>().Play("fade");
        }
    }
}
