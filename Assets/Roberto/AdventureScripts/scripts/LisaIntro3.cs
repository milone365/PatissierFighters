using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LisaIntro3 : MonoBehaviour
{
    LisaADV_Intro lisa;
    [SerializeField]
    ADV_Intro_Robots greenRobot=null,brownRobot=null;
    [SerializeField]
    Transform point3=null, shotPoint=null;
    
    //キャラクターに戻る命令を送って、ロボットも移動
    void Start()
    {
        lisa = FindObjectOfType<LisaADV_Intro>();
        lisa.comeBack();
        Invoke("moveRobot", 2.5f);
    }
    void moveRobot()
    {
        greenRobot.addPointToList(point3);
        brownRobot.addPointToList(shotPoint);
    }
}
