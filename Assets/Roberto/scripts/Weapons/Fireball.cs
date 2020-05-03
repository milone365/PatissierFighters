using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : WPbullet
{
    [SerializeField]
    GameObject fire=null;
    //Critical Hitの場合は、火傷を付ける
    public override void Effect(Collider c, B_Player p)
    {
        int rnd = Random.Range(1, 11);
        if(rnd>=9)
        {
            //Interfaceを呼ぶ
            IBurn b = c.GetComponent<IBurn>();
            if (b == null) return;
            b.burn();
            if (fire == null) return;
            GameObject newFire= Instantiate(fire, transform.position, transform.rotation)as GameObject;
            newFire.transform.SetParent(c.transform);
            newFire.transform.localPosition=new Vector3(0, 0, 0);

        }
    }
}
