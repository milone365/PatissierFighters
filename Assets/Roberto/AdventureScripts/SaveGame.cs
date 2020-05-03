using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public string gametype = "Adventure";
 
    
    public static void SaveINTdata(string st, int v)
    {
        PlayerPrefs.SetInt(st, v);
    }
    //スコアとｈｐ保存する
    public void saveGame()
    {
        ADV_Player player = FindObjectOfType<ADV_Player>();
        if (player == null) return;
        int h = (int)player.Get_Player_Health().getHealth();
        SaveINTdata(StaticStrings.savedHealth, h);
        SaveINTdata(gametype+StaticStrings.savedScore, ADV_UIManager.getScore());

    }
}
  
