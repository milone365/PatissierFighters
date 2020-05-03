using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using M;
using td;
using System;

public class Mymanager :NetworkManager
{
    public float startTime = 44;
    public MU_TD_GameManager manager;
    public GameObject video;
    public int levelNumber = 35;
    public bool isRugby=false;
    public GameObject[] players;
    public Transform[] cube = null;
    bool isStarted = false;
    public float timer = 10;
    public int index = 0;
    public int playerIndex = 0;
    public GameObject panel;
    InputField inputfield;

    private void Update()
    {
        if (isStarted) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isStarted = true;
            M_GameManager gm = FindObjectOfType<M_GameManager>();
           
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        playerPrefab = players[playerIndex];
        Quaternion rot = new Quaternion(0, cube[index].transform.rotation.y, 0,0);
        GameObject newPlayer = (GameObject)Instantiate(playerPrefab, cube[index].position, rot);
        if (isRugby)
        {
            getTeamToPlayer(newPlayer,index);
        }
        NetworkServer.AddPlayerForConnection(conn, newPlayer, playerControllerId);
        index++;
        playerIndex++;
        if (index >= cube.Length)
        {
            index = 0;
        }
        if (video != null && !video.activeInHierarchy)
        {
            video.SetActive(true);
        }
        if (panel != null && panel.activeInHierarchy)
        {
            panel.SetActive(false);
        }
        Invoke("activeGame", startTime);
    }
    void getTeamToPlayer(GameObject obj,int value)
    {
        Team newTeam;
        if (value % 2 == 0)
        {
            newTeam = Team.chocolate;
        }
        else
        {
           newTeam = Team.vanilla;
        }
        obj.GetComponent<SetupLocalPlayer>().team = newTeam;
    }
    void activeGame()
    {
        manager.StartGame();
    }
}
