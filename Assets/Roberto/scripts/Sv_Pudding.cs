using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sv_Pudding : MonoBehaviour,IHealth
{
    [SerializeField]
    float hp = 50;
    bool isEated = false;
    SV_Tower tower;
    float damageCounter = 1.5f;
    float damageTime = 1.5f;
    [SerializeField]
    float attractTime = 10;
    float attractTimeDelay = 10;
    public void Healing(float heal)
    {
        throw new System.NotImplementedException();
    }

    public void takeDamage(float damageToTake)
    {
        if (isEated) return;
        hp -= damageToTake;
        if (hp <= 0)
        {
            isEated = true;
            SV_EnemyController[] controller = FindObjectsOfType<SV_EnemyController>();
            foreach (var e in controller)
            {
                if (e != null)
                    e.changeTarget(tower.transform);
            }
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tower = FindObjectOfType<SV_Tower>();
        SV_EnemyController[] controller = FindObjectsOfType<SV_EnemyController>();
        foreach(var e in controller)
        {
            if (e != null)
                e.changeTarget(this.transform);
        }
    }
    //敵を引き付けるそしてタイマーに合わせてダメージをつける
    private void Update()
    {
        damageCounter -= Time.deltaTime;
        attractTime -= Time.deltaTime;
        if (damageCounter <= 0)
        {
            damageCounter = damageTime;
            //raycast で敵ｈｐにアクセスする
            Collider[] allEnemies = Physics.OverlapSphere(transform.position, 5);
            foreach(var e in allEnemies)
            {
                SV_EnemyHealth h = e.GetComponent<SV_EnemyHealth>();
                if (h != null)
                {
                    h.takeDamage(1);
                }
            }
        }
        //引き付ける
        if (attractTime <= 0)
        {
            attractTime = attractTimeDelay;
            SV_EnemyController[] controller = FindObjectsOfType<SV_EnemyController>();
            foreach (var e in controller)
            {
                if (e != null)
                    e.changeTarget(this.transform);
            }
        }
    }
}
