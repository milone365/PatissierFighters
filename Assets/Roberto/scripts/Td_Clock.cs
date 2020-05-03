using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace td
{

    public class Td_Clock : MonoBehaviour
    {
        bool gameEnd = false;
        public bool GaneStart { get { return gameStart; } set { gameStart = value; } }
        Text clockText;
        [SerializeField]
        float battleTimer = 180;

        bool gameStart = false;
        private void Start()
        {
            clockText = GetComponentInChildren<Text>();
            clockText.text = "180";
        }

        //タイマー
        void Update()
        {
            if (gameEnd || !gameStart) return;

            battleTimer -= Time.deltaTime;
            if (battleTimer <= 0)
            {
                battleTimer = 0;
                finalCountDown();
            }
            clockText.text = Mathf.FloorToInt(battleTimer).ToString();

        }

        //タイマー時間が終わったら終了
        void finalCountDown()
        {
            gameEnd = true;
            TD_GameManager gm = FindObjectOfType<TD_GameManager>();
            StartCoroutine(gm.gameEndCo());

        }
    }

}


