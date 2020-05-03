using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoBullet : WPbullet
{
    // particle effect
    public override void Effect(Collider c, B_Player p)
    {
        ITornado tornado = c.GetComponent<ITornado>();
        if (tornado != null)
        {
             tornado.Vortex();
        }
    }
}
