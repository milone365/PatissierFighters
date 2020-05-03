using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : ADV_RotatorOnShot
{
    [SerializeField]
    GameObject platforms=null;
    SpriteRenderer arrow;
    ADV_Player player;
    [SerializeField]
    GameObject c_amera = null;
    GameObject jumpingPoint;


    void Start()
    {
        arrow = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(activationCO());
    }
    //橋をアクティブする
    public override void Interaction()
    {
        if (death) return;
        hp--;
        if (hp <= 0)
        {
            death = true;
            transform.rotation = newRotation;
            c_amera.SetActive(true);
            platforms.GetComponent<Animator>().SetTrigger("UP");
            Destroy(arrow.gameObject);
        }
        
    }
    //referenceを貰う
    public void passPlayerAdJumpPoint(GameObject obj,GameObject jp_point)
    {
        player = obj.GetComponent<ADV_Player>();
        jumpingPoint = jp_point;
    }
    //道にポイントを追加
    IEnumerator activationCO()
    {
        yield return new WaitForSeconds(2);
        List<Transform> route = new List<Transform>();
        route.Add(jumpingPoint.transform);
        player.GetComponent<RouteFollower>().changeRoute(route);
    }
}
