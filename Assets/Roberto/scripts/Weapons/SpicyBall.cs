using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpicyBall : Bomb
{
    //interfaceを呼び出す
    public override void Effect(Collider c, B_Player p)
    {
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.SPICESKILL);
        IConfused confused = c.GetComponent<IConfused>();
        if (confused != null)
        {
            confused.confuse();
        }
    }
}
