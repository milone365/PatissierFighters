using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Waypoints : MonoBehaviour
{
    public static ADV_Waypoints instance;
    public List<Transform> points;
    public Transform[] route1;
    public List<Transform> route2;
    void Awake()
    {
        instance = this;
        points = new List<Transform>();
        foreach (var t in route1) 
        {
            points.Add(t);
        }
    }

    public void addPointToRoute(List<Transform> list) 
    {
        foreach (var p in list)
        {
            points.Add(p);
        }
    }

    public void Getnewroute(List<Transform>r)
    {
        points = r;
        Debug.Log("r2");
    }
}
