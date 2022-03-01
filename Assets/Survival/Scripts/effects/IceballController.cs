using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;
public class IceballController : WPbullet
{
    public SnowMan snowMan;
    Transform camera=null;
    public override void Effect(Collider c, B_Player p)
    {
        bool isAi=true;
        Inputhandler hand = c.GetComponent<Inputhandler>();
        if (hand != null)
        {
            isAi = false;
            camera = c.GetComponentInChildren<Camerahandler>().GetComponent<Transform>();
            if(camera!=null)
            camera.SetParent(null);
        }
        IFroze froze_stat = c.GetComponent<IFroze>();
        if(froze_stat != null)
        {
           SnowMan sman = Instantiate(snowMan, c.transform.position, c.transform.rotation)as SnowMan;
           sman.objectToFreeze(c.gameObject, isAi,camera);
            
        }
    }
}
