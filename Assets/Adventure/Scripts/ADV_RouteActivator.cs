using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_RouteActivator : MonoBehaviour,IShotable
{
    public RoutePassType type;
   
    bool Activate = false;
    public List<Transform> points = new List<Transform>();
    RouteFollower follower;
    private void Start()
    {
        RouteFollower[] followers = FindObjectsOfType<RouteFollower>();
        foreach (var f in followers)
        {
            if (f.gameObject.GetComponent<ADV_Player>())
            {
                follower = f;
            }
        }
        if (type == RoutePassType.onEnable)
        {
            Activate = true;
            follower.changeRoute(points);
        }
    }
    public void interact(Vector3 hitPos)
    {
        if (Activate) return;
        Activate = true;
        switch (type)
        {
            case RoutePassType.add:
                //ADV_Waypoints.instance.addPointToRoute(points);
                follower.addRoute(points);

                break;
            case RoutePassType.change:
                //ADV_Waypoints.instance.Getnewroute(points);
                follower.changeRoute(points);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (type != RoutePassType.onTrigger) return;
        if (other.tag == StaticStrings.player)
        {
            follower.changeRoute(points);
        }
    }
    public enum RoutePassType
    {
        add,
        change,
        onTrigger,
        onEnable
     }
}
