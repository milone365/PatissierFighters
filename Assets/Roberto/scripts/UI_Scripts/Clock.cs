using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    bool gameEnd = false;
    public bool GaneStart { get { return gameStart; } set { gameStart = value; } }
    Text clockText;
    [SerializeField]
    float battleTimer = 180;
    float countdowntimer;
    bool gameStart = false;
    bool final = false;
    private void Start()
    {
        clockText = GetComponentInChildren<Text>();
        clockText.text = "180";
    }

    //タイマー
    void Update()
    {
        if (gameEnd||!gameStart) return;
        
         battleTimer -= Time.deltaTime;
        countdowntimer = battleTimer;
        if (battleTimer <= 0)
        {
            gameEnd = true;
            battleTimer = 0;
            PlayerCanvas[] allcanvas = FindObjectsOfType<PlayerCanvas>();
            foreach (var item in allcanvas)
            {
                item.gameObject.SetActive(false);
            }

        }
        clockText.text = Mathf.FloorToInt(battleTimer).ToString();
        if (final) return;
        if (countdowntimer <= 4)
        {
            final = true;
            finalCountDown();
        }
        
         
    }

    //タイマー時間が終わったら終了
    void finalCountDown()
    {
            clockText.fontSize = 20;
            B_GameManager.instance.StartcountDown(false);
        
    }
}
