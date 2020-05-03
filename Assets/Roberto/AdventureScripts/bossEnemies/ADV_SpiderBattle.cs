using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_SpiderBattle : MonoBehaviour
{
    
   
    GameObject camera2 = null;
    [SerializeField]
    GameObject broom = null,jumpingPoint=null;
    ADV_Player player;
    float spawningTime = 1.5f;
    [SerializeField]
    float lifeTime = 30;
    float sp_time;
    [SerializeField]
    GameObject spider = null;
    [SerializeField]
    Transform[] spawnPoints=null;
    bool stop = false;
    ADV_SpiderBoss boss;
    [SerializeField]
    GameObject box = null;
    ADV_Boss_Platforms[] allPlatforms;
    void Start()
    {
        player = GetComponentInChildren<ADV_Player>();
        sp_time = spawningTime;
        boss = FindObjectOfType<ADV_SpiderBoss>();
        boss.gameObject.SetActive(false);
        camera2 = transform.GetChild(0).gameObject;
        allPlatforms = GetComponentsInChildren<ADV_Boss_Platforms>();
        foreach (var t in allPlatforms)
        {
            t.gameObject.SetActive(false);
        }

    }

    //時間が経ったら敵を作る

    void Update()
    {
        if (stop) return;
        sp_time -= Time.deltaTime;
        lifeTime -= Time.deltaTime;
        if (sp_time <= 0)
        {
            sp_time = spawningTime;
            spawn();
        }
        if (lifeTime <= 0)
        {
            stop = true;
           StartCoroutine(ActiveBoss());
        }

    }
    //時間が終ったらボースバトルが始まる
    IEnumerator ActiveBoss()
    {
        camera2.SetActive(true);
        yield return new WaitForSeconds(5);
       if (EffectDirector.instance != null)
        {
            EffectDirector.instance.playInPlace(box.transform.position,StaticStrings.bomb);
        }
       //platformが使えるように処理
        
        foreach(var t in allPlatforms)
        {
            if (t != null)
            {
                t.gameObject.SetActive(true);
            }
        }
        Destroy(box);
        boss.gameObject.SetActive(true);
        boss.passObjects(broom, jumpingPoint);
        //イベント
        boss.onBossDeath += onbossDeath;
    }
    
    void onbossDeath()
    {
        foreach (var t in allPlatforms)
        {
            if (t != null)
            {
                t.gameObject.SetActive(false);
            }
        }
        camera2.SetActive(false);
    }
    void spawn()
    {
        int rnd = UnityEngine.Random.Range(0, spawnPoints.Length);
        Instantiate(spider, spawnPoints[rnd].position, Quaternion.identity);
    }
}
