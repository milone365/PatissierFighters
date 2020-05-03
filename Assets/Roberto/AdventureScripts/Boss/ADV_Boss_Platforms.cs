using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Boss_Platforms : MonoBehaviour,IShotable
{
    [SerializeField]
    Transform land = null;
    ADV_SpiderBoss spider;
    public float speed = 2;
    public float height = 2;
    RouteFollower follower;
    [SerializeField]
    float spiderAccuracy = 5;
    private void Start()
    {
        follower = FindObjectOfType<RouteFollower>();
    }
    //プレーヤーの飛び
    public void interact(Vector3 hitPos)
    {
        follower.GetComponent<Rigidbody>().isKinematic = true;
        follower.gameObject.GetComponent<Animator>().SetTrigger(StaticStrings.Jump);
        follower.JumpToPoint(land, height, speed);
       /* if (spider != null)
        {
            
            float rnd = Random.Range(2f, 4f);
            Invoke("moveSpider", rnd);
        }*/
     }
    public void passBoss(ADV_SpiderBoss boss)
    {
        spider = boss;
    }
    //ボース飛ぶ
    void moveSpider()
    {
        float distance = Vector3.Distance(follower.transform.position, transform.position);
        if (distance > spiderAccuracy) return;
        spider.addArcpointToRoute(this.transform, height, speed);
    }
}
