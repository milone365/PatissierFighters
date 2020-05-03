using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaramelBomb : WPbullet
{
    [SerializeField]
    GameObject gravityZone = null;
    //引力範囲を作る
    public override void Effect(Collider c, B_Player p)
    {
        Instantiate(gravityZone, transform.position, transform.rotation);
    }
}
