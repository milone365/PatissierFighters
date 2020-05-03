using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_TossPancakeBall : MonoBehaviour
{
    [SerializeField]
    Transform center = null;
    [SerializeField]
    GameObject pancake = null;
    [SerializeField]
    Transform spawnpoint = null;
    ADV_Boss boss;
    Transform player;
    [SerializeField]
    float bulletSpeed=50;
    private void Start()
    {
        boss = GetComponentInParent<ADV_Boss>();
        if (boss == null) return;
        player = boss.Get_player();
        
    }
    //バレットを作る
    public void spawnPancake()
    {
        GameObject newPancake = Instantiate(pancake, spawnpoint.position, spawnpoint.rotation);
        newPancake.GetComponent<ADV_ReversableBullet>().shoot(center,player, bulletSpeed);
    }
    //プレーヤーreferenceをもらう
    public void setPlayer(Transform t)
    {
        player = t;
    }
}
