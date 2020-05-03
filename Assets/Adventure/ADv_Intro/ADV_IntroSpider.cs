using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_IntroSpider : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    Transform objective = null;
    [SerializeField]
    float speed = 5;
    Vector3 dir;
    [SerializeField]
    float attackposition = 1;
    [SerializeField]
    float attackDealy = 3;
    float attackCounter;
    bool die = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        attackCounter = attackDealy;
    }

    //移動とアニメーション
    void Update()
    {
        if (objective == null||die) return;
        dir = objective.position- transform.position ;
        float distance = Vector3.Distance(transform.position, objective.position);
        if (distance > attackposition)
        {
            transform.position += dir.normalized * speed * Time.deltaTime;
            anim.SetFloat("MOVE", distance);
        }
        else
        {
            anim.SetFloat("MOVE", 0);
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0)
            {
                attackCounter = attackDealy;
                anim.SetTrigger("ATTACK");
            }
            
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (!die)
                die = true;
            anim.SetTrigger("Die");
        }
    }
}
