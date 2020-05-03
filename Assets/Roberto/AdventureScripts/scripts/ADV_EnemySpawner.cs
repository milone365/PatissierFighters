using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_EnemySpawner : ADV_Route
{
  
    [SerializeField]
    float spawnTime = 2;
    [SerializeField]
    GameObject enemy = null;
    Transform player = null;
    public override void INIT()
    {
        player = FindObjectOfType<ADV_Player>().transform;
        Transform[] alltransforms = GetComponentsInChildren<Transform>();
        foreach (var t in alltransforms)
        {
            if (t != this.transform)
            {
                points.Add(t);
            }
        }
        InvokeRepeating("spawn", spawnTime, spawnTime);
    }
   //generate enmy and pass routePoints
    public void spawn()
    {
        int rand = Random.Range(0, points.Count - 1);
        GameObject e = Instantiate(enemy, points[rand].position, Quaternion.identity);
        RouteFollower follower = e.GetComponent<RouteFollower>();
        if (follower != null)
        {
            follower.changeRoute(points);
            follower.route_type = RouteFollower.RouteType.random;
        }
    }
}
