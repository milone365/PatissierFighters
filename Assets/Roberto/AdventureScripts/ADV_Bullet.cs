using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Bullet : MonoBehaviour
{
    

    float bulletForce = 0;
    Transform target;
    Vector3 startDirection;
    
    private void Start()
    {
        Destroy(gameObject, 10);
    }
   
    private void Update()
    {
        GetComponent<Rigidbody>().AddForce(startDirection.normalized * bulletForce * Time.deltaTime, ForceMode.VelocityChange);
    }
    public void shoot(Transform player, float force)
    {
        
        bulletForce = force;
        target = player;
        startDirection = target.position - transform.position;
        transform.LookAt(target);
    }
    public void SpreadShoot(Vector3 d,float s)
    {
        bulletForce = s;
        startDirection = -d;
    }
    //ダメージ
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            IHealth h = other.GetComponent<IHealth>();
            h.takeDamage(1);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, "Spark");
            }
            Destroy(gameObject);

        }
    }
}
