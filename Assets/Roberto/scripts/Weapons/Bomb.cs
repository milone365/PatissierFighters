using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Bomb : WPbullet
{
    //爆弾
    public override void Effect(Collider c, B_Player p)
    {
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.bomb);
        //飛ぶを呼び出す
        IJump jmp = c.GetComponent<IJump>();
        if (jmp!=null)
        {
            jmp.jump();
        }
        if (Soundmanager.instance == null) return;
        Soundmanager.instance.PlaySeByName(StaticStrings.explosion);
    }

    public override void InitializinEffect()
    {
       
        Invoke("activeCollider",0.1f);
    }

    void activeCollider()
    {
        GetComponent<Collider>().enabled = true;
    }


}
