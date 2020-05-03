using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerMulti : MonoBehaviour
{
    public float time;
    Text timerText;
    void Start()
    {
        timerText = GetComponentInChildren<Text>();
        timerText.text = "";
    }
    void Update()
    {
        time -= Time.deltaTime;
        timerText.text = time.ToString();
    }
}
