using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LastBuletDefinitive : NetworkBehaviour
{
    Vector3 dir = Vector3.zero;
    [HideInInspector]
    public Collider c;
    public string effectname = "CREAMHIT";
    public int damage = 1;
    public virtual void init()
    {
        InitializingEffect();
        Destroy(gameObject, 5);
    }
    private void Update() 
    {
        UpdateBullet();
    }
    public virtual void UpdateBullet()
    {
        if (dir == Vector3.zero)
        {
            dir = transform.forward;
        }
        GetComponent<Rigidbody>().velocity = dir * 50;
    }
    public void getDirection(Vector3 direction) 
    {
       dir = direction;
        
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.helper || other.tag == StaticStrings.player
            || other.tag == StaticStrings.tower || other.tag == StaticStrings.AI || other
            .tag == StaticStrings.cpu)
        {
            IHealth h = other.GetComponent<IHealth>();
            if (h != null)
            {
                h.takeDamage(damage);
            }
            Effect(other);
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName(StaticStrings.Splat1);

            Destroy(gameObject);
        }

    }
    public virtual void Effect(Collider c)
    {
        if (EffectDirector.instance != null)
            EffectDirector.instance.playInPlace(transform.position, effectname);
    }
    public virtual void InitializingEffect()
    {

    }
    public void Toss(Vector3 direction)
    {
        dir = direction;
        GetComponent<Rigidbody>().velocity = dir * 50;
    }
}
