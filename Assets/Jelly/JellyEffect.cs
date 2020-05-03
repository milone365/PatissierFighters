using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class JellyEffect : MonoBehaviour
{
    [SerializeField]
    float disappearTime = 10;
    bool destroy = false;
    SV_EnemyController enemy;
    private void Update()
    {
        disappearTime -= Time.deltaTime;
        if (disappearTime <= 0)
        {
            if (!destroy)
            {
                destroy = true;
                enemy.Unblock();
                Destroy(gameObject);
            }
        }
    }
    public void giveEnemy(SV_EnemyController controller)
    {
        enemy = controller;
    }
}
