using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SV_EnemyController : MonoBehaviour,ITornado
{
    float flyHeight = 1f;
    float vortexTime = 8;
    [SerializeField]
    ParticleSystem tornado = null;
    bool isInTheVortex = false;
    Vector3 vortexPos;
    Quaternion vortexRotation;
    NavMeshAgent agent;
    Animator anim;
    bool isDead = true;
    SV_EnemyHealth health;
    bugAction currentaction;
    Transform target;
    SV_Tower tower;
    [SerializeField]
    float speed = 10;
    float attackRange = 25;
    [SerializeField]
    float attackDelay=1, attackpower=1;
    float attacCounter;
    Rigidbody rb = null;
    float gravityScale = 10f;
    bool initialized = false;
    public void INITIALIZE(SV_Tower t)
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<SV_EnemyHealth>();
        attacCounter = attackDelay;
        agent.speed = speed;
        tower = t;
        target = tower.transform;
        isDead = false;
        health.onDeath += onDeath;
        rb = GetComponent<Rigidbody>();
        initialized = true;
    }
    private void Update()
    {
        if (!initialized) return;
        if (isDead||tower==null) return;
        anim.SetFloat(StaticStrings.move, agent.velocity.magnitude);
        updating();
        
    }
    void updating()
    {
        switch (currentaction)
        {
            case bugAction.chase:
                Chasing();
                break;
            case bugAction.attack:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        if (agent.destination != transform.position)
            agent.SetDestination(transform.position);
        if (target == null) return;
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            changeState(bugAction.chase);
        }
        attacCounter -= Time.deltaTime;
        if (attacCounter <= 0)
        {
            attacCounter = attackDelay;
            anim.SetTrigger("ATK");
            IHealth h = target.GetComponent<IHealth>();
            h.takeDamage(attackpower);
          
        }
    }

    private void Chasing()
    {
        if (target == null) return;
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            changeState(bugAction.attack);
        }
    }

    public void changeState(bugAction ac)
    {
        currentaction = ac;
    }
    public void changeTarget(Transform t)
    {
        target = t;
        if (target == tower.transform)
        {
            attackRange = 25;
        }
        else
        {
            attackRange = 10;
        }
            
        //changeState(bugAction.chase);
    }

    void onDeath()
    {
        isDead = true;
        if(agent.enabled==true)
        agent.ResetPath();
        anim.SetTrigger(StaticStrings.death);
        health.onDeath -= onDeath;
        Destroy(gameObject, 3);
    }

    public void Block()
    {
        isDead = true;
        agent.enabled = false;
    }
    public void Unblock()
    {
        agent.enabled = true;
        isDead = false;
    }

    public void Vortex()
    {
        if (isInTheVortex) return;
        isInTheVortex = true;
        vortexPos = transform.position;
        vortexRotation = transform.rotation;
        if (tornado != null) tornado.Play();
        initialized = false;
        agent.ResetPath();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.mass = 0;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(Vector3.up, ForceMode.Impulse);
        gravityScale = -10;
    }
    void vorterxUpdate()
    {

        transform.Translate(0, vortexPos.y + flyHeight, 0, Space.World);
        flyHeight += Time.deltaTime;

        vortexTime -= Time.deltaTime;
        if (vortexTime <= 0)
        {
            isInTheVortex = false;
            vortexTime = 8;
            rb.isKinematic = true;
            rb.useGravity = true;
            rb.mass = 1;
            gravityScale = 10;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            initialized = true;
            transform.rotation = vortexRotation;
            if (tornado != null) tornado.Stop();
            flyHeight = 1f;
        }
    }
}

public enum bugAction
{
    chase,
    attack
}
