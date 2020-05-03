using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text winnerText=null;

    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    　　winnerText.text="";
    }
    //メッセージ
    public void ShowWinnerText(string tx)
    {
        winnerText.text = tx;
    }
}
