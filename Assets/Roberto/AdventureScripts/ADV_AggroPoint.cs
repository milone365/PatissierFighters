using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_AggroPoint : MonoBehaviour
{

    public List<ADV_Enemies> enemies = new List<ADV_Enemies>();
    bool active=false;
    void Start()
    {
        if (enemies.Count < 1)
        {
            ADV_Enemies[] allEnemies = FindObjectsOfType<ADV_Enemies>();
            foreach(var e in allEnemies)
            {
                enemies.Add(e);
            }
        }
    }

    //プレーヤーを狙う為
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (active) return;
            active = true;
            foreach(var e in enemies)
            {
                e.follow(other.transform);
            }
        }
    }
}
