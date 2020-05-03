using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBullet : WPbullet
{
    [SerializeField]
    GameObject jelly = null;
    public override void Effect(Collider c, B_Player p)
    {

        Instantiate(jelly, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
