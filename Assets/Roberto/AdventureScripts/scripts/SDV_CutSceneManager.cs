using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDV_CutSceneManager : MonoBehaviour
{
    [SerializeField]
    Transform[] points = null;
    List<ADV_Intro_Robots> robots = new List<ADV_Intro_Robots>();
    public void AddRobot(ADV_Intro_Robots r)
    {
        robots.Add(r);
    }
 //ロボットにポイントを渡す
    public void guideRobot()
    {
        for (int i = 0; i < points.Length; i++)
        {
            robots[i].addPointToList(points[i]);
        }
    }
}
