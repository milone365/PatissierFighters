using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Route : MonoBehaviour
{
   protected List<Transform> points = new List<Transform>();

    //道を渡す
    private void Start()
    {
        Invoke("INIT", 2);
    }
    public virtual void INIT()
    {
        Transform[] alltransforms = GetComponentsInChildren<Transform>();
        foreach (var t in alltransforms)
        {
            if (t != this.transform)
            {
                points.Add(t);
            }
        }
        RouteFollower follower = FindObjectOfType<RouteFollower>();
        follower.changeRoute(points);
        follower.FindDestination();
    }
}
