using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adv_FlyngPlatform : MonoBehaviour,IShotable
{
    [SerializeField]
    Transform land = null;
    RouteFollower follower;
    public float speed = 2;
    public float height = 2;
    bool active = false;
    [SerializeField]
    bool lastPlatform = false;
    public List<Transform> points=new List<Transform>();
    public bool rotable = true;
    void Start()
    {
        follower = FindObjectOfType<ADV_Player>().GetComponent<RouteFollower>();
    }

  
    public void interact(Vector3 hitPos)
    {
        //回る
        if (!active)
        {
            active = true;
            if(rotable)
            transform.rotation = new Quaternion(180, 0, 0, 0);
            EffectDirector.instance.EffectAndPopup(hitPos, "AURABUBBLE", 30);
        }
        //プレーヤーを飛ぶ
        follower.GetComponent<Rigidbody>().isKinematic = true;
        follower.gameObject.GetComponent<Animator>().SetTrigger(StaticStrings.Jump);
        follower.JumpToPoint(land, height, speed);
        follower.transform.SetParent(this.transform);
        if (lastPlatform)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }


}
