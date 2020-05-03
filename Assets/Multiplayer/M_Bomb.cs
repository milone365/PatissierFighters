using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Bomb : LastBuletDefinitive
{
    private void Start()
    {
        InitializingEffect();
        Destroy(gameObject, 5);
    }
    //爆弾
    public override void Effect(Collider c)
    {
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.bomb);
        //飛ぶを呼び出す
        IJump jmp = c.GetComponent<IJump>();
        if (jmp != null)
        {
            jmp.jump();
        }
        if (Soundmanager.instance == null) return;
        Soundmanager.instance.PlaySeByName(StaticStrings.explosion);
    }

    public override void UpdateBullet()
    {
       
    }
    public override void InitializingEffect()
    {

        Invoke("activeCollider", 0.1f);
    }

    void activeCollider()
    {
        GetComponent<Collider>().enabled = true;
    }
}
