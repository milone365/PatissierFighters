using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Bullet : LastBuletDefinitive
{
    public override void Effect(Collider c)
    {
        if (EffectDirector.instance != null)
            EffectDirector.instance.playInPlace(transform.position, effectname);
    }
}
