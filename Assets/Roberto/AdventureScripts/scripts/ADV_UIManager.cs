using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADV_UIManager : MonoBehaviour
{
    
    [SerializeField]
    Image[] icons = null;
    [SerializeField]
    Text pointText=null;
    static int score = 0;
    public int levelScore = 0;

    public static int getScore()
    {
        return score;
    }
    private void Start()
    {
        if(EffectDirector.instance!=null)
        EffectDirector.instance.setUImanager(this);
        if (PlayerPrefs.HasKey(StaticStrings.savedScore))
        {
            score = PlayerPrefs.GetInt("Adventure"+StaticStrings.savedScore);
        }
        if (pointText == null) return;
        pointText.text = score.ToString();
    }
    //イベント
    public void OnChangingHealth(float v)
    {
        int value;
        if (v == 0)
        {
            value = 0;
        }
        else
        {
            value = (int)v - 1;
        }
        activeStrawberry(value);
         
    }
    //ｈｐ画像
    void activeStrawberry(int amount)
    {
        for(int i = 0; i < icons.Length; i++)
        {
            if (i <= amount)
            {
                icons[i].enabled = true;
            }
            else
            {
                icons[i].enabled = false;
            }
        }
        if (amount == 0)
        {
            icons[0].enabled = false;
        }
    }
    //スコア
    public void addPoints(int p)
    {
        if (pointText == null) return;
        score += p;
        levelScore += p;
        if (score <= 0)
        {
            score = 0;
            levelScore = 0;
        }
        pointText.text = score.ToString();
    }
    public void resetLevelScore()
    {
        score -= levelScore;
        if (pointText == null) return;
        pointText.text = score.ToString();
    }
}