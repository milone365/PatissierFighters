using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankSystem : MonoBehaviour
{
    public bool resetAll = false;
    [SerializeField]
    Text[] playerNamexTxt=null;
    [SerializeField]
    Text[] playerScoreTxt = null;
    public gameType type=gameType.adventure;
    public List<PlayerDetails> allPlayers;
    string gmType;
    int currentPlayer;
    private void Start()
    {
        switch (type)
        {
            case gameType.adventure:
                gmType = "Adventure";
                break;
            case gameType.survival:
                gmType = "Survival";
                break;
        }
        allPlayers = new List<PlayerDetails>();
        foreach(var p in playerNamexTxt)
        {
            PlayerDetails d = new PlayerDetails();
            allPlayers.Add(d);
        }
        for (int i=0;i<allPlayers.Count;i++)
        {
            if (PlayerPrefs.HasKey(gmType + StaticStrings.savedScore+i.ToString()))
            {
                
                //load all playerScores and names
                allPlayers[i].playerScore = PlayerPrefs.GetInt(gmType+StaticStrings.savedScore + i.ToString());
                allPlayers[i].PlayerName = PlayerPrefs.GetString(gmType + StaticStrings.playerName + i.ToString());
                playerNamexTxt[i].text = allPlayers[i].PlayerName;
                playerScoreTxt[i].text = allPlayers[i].playerScore.ToString();
            }
            
            //今回のスコアと保存されたスコアを比べる
            if (allPlayers[i].playerScore < PlayerPrefs.GetInt(gmType+StaticStrings.savedScore))
            {
                //current player score
                int score = PlayerPrefs.GetInt(gmType + StaticStrings.savedScore);
                PlayerPrefs.SetInt(gmType + StaticStrings.savedScore + i.ToString(), score);
                PlayerPrefs.SetString(gmType + StaticStrings.playerName+i.ToString(),PlayerPrefs.GetString(StaticStrings.playerName));
                currentPlayer =  i;
                playerNamexTxt[i].text = PlayerPrefs.GetString(StaticStrings.playerName);
                playerScoreTxt[i].text = score.ToString();
                break;
            }
           
        }

        if (resetAll)
        {
            for(int i=0;i< allPlayers.Count; i++)
            {
                PlayerPrefs.SetInt(gmType + StaticStrings.savedScore + i.ToString(), 0);
                PlayerPrefs.SetString(gmType + StaticStrings.playerName + i.ToString(), "AAA");
                playerNamexTxt[i].text = "AAA";
                playerScoreTxt[i].text = "0";
            }
           
        }
    }
    private void Update()
    {
        Blinking();
    }

    private void Blinking()
    {
        
    }
}

public enum gameType
{
    adventure,
    survival
}
[System.Serializable]
public class PlayerDetails
{
    public string PlayerName="AAA";
    public int playerScore=0;
}
