using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using td;

public class WPbullet :MonoBehaviour
{
    B_Player attacker;
    public float damage=1;
    public string effectname=null;
    Vector3 direction = Vector3.zero;
   
    protected float bulletForce=250;
    public void Start()
    {
        Destroy(gameObject, 5);
        InitializinEffect();
        if (effectname != null) return;
        effectname = StaticStrings.CREAMHIT;
    }
    private void Update()
    {
        GetComponent<Rigidbody>().AddForce(direction.normalized * bulletForce * Time.deltaTime,ForceMode.VelocityChange);
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.helper || other.tag == StaticStrings.player
            || other.tag == StaticStrings.tower|| other.tag == StaticStrings.AI||other
            .tag==StaticStrings.cpu)
        {
               IHealth h = other.GetComponent<IHealth>();
                if (h != null)
                {
                    h.takeDamage(damage);
                }

            if (attacker != null)
                attacker.Add_SkillPoints(1);
            Effect(other, attacker);
            if(Soundmanager.instance!=null)
            Soundmanager.instance.PlaySeByName(StaticStrings.Splat1);
          
            Destroy(gameObject);
        }
       
    }

    public void passDirection(B_Player obj, Vector3 d)
    {
        direction = d;
        attacker = obj; 
    }
    public void passDirection(Vector3 d)
    {
        direction = d;
    }

    public virtual void Effect(Collider c, B_Player p)
    {
        if(EffectDirector.instance!=null)
        EffectDirector.instance.playInPlace(transform.position, effectname);
    }
    public virtual void InitializinEffect()
    {

    }

}
