using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_ReversableBullet : MonoBehaviour,IShotable

{
    Transform robot;
    int hp = 1;
    float bulletForce = 0;
    Transform target;
    Vector3 startDirection;
    Transform player;
    private void Start()
    {
        Destroy(gameObject, 10);
    }
    //ｈｐはゼロになったら、後ろに戻る
    public void interact(Vector3 hitpos)
    {
        hp--;
        if (hp <= 0)
        {
            
            target = robot;
            if (robot != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                startDirection = robot.position - transform.position; 
            }
            
            bulletForce *= 2;
        }
       
    }
    //移動
    private void Update()
    {

        GetComponent<Rigidbody>().AddForce(startDirection.normalized * bulletForce * Time.deltaTime, ForceMode.VelocityChange);
    }
    //方向、と力、とターゲットを渡す関数
    public void shoot(Transform shooter,Transform player,float force)
    {
        robot = shooter;
        bulletForce = force;
        target = player;
        this.player = player;
        startDirection = target.position - transform.position;
        transform.LookAt(target);
       
      
    }
    //ダメージを付ける
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            IHealth h = other.GetComponent<IHealth>();
            h.takeDamage(1);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.CREAMHIT);
            }
            Destroy(gameObject);

        }
        if (other.tag == "Boss")
        {
            if (hp > 0)
                return;
            IEnemy h = other.GetComponent<IEnemy>();
            h.takeDamage(10);
            EffectDirector.instance.playInPlace(transform.position, "HELPERMELEE");
            Destroy(gameObject);
        }
    }
}
