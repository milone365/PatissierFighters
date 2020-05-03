using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class M_Clock : NetworkBehaviour
{
    bool gameEnd = false;
  
    Text clockText;
   [SyncVar]
    public float battleTimer = 180;
    bool gameStart = false;
    private void Start()
    {
        clockText = GetComponentInChildren<Text>();
        clockText.text = "180";
    }

    void Update()
    {
        if (gameEnd||!gameStart) return;
        
         battleTimer -= Time.deltaTime;
        if (battleTimer <= 0)
        {
            battleTimer = 0;
            finalCountDown();
        }
         clockText.text = Mathf.FloorToInt(battleTimer).ToString();
         
    }
    public void startGame()
    {
        gameStart = true;
    }
    [Command]
   public  void CmdStartgame()
    {
        startGame();
        RpcStart();
    }
    [ClientRpc]
    void RpcStart()
    {
        if (!isServer)
        {
            startGame();
        }
    }
    //タイマー時間が終わったら終了
    void finalCountDown()
    {
        gameEnd = true;
        clockText.fontSize = 20;
        M_GameManager g = FindObjectOfType<M_GameManager>();
        g.EndGame();
    }
}
