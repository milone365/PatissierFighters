using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;


public class CPUBrain : MonoBehaviour, ITeam, IFroze, IJump, ITornado
{
    
    float flyHeight = 1f;
    float vortexTime = 8;
    [SerializeField]
    ParticleSystem tornado = null;
    bool isInTheVortex = false;
    CapsuleCollider coll = null;
    Team tm;
    public CpuType aiType;
    PlayerCPU cpu = null;
    B_Player player;
    public Animator anim;
    NavMeshAgent ag;
    STATUS status;
    bool initialized = false;
    [SerializeField]
    Transform hand = null;
    [SerializeField]
    WPbullet bomb = null;
    float frozingTime = 5;
    Rigidbody rb = null;
    public bool isfrozen { get; set; }
    float power = 50;
    Vector3 dir = Vector3.zero;
    float gravityScale = 10f;
    Vector3 vortexPos;
    Quaternion vortexRotation;
    #region init
    public void INIT(B_Player p)
    {

        anim = GetComponent<Animator>();
        player = p;
        ag = GetComponent<NavMeshAgent>();
        status = new STATUS();
        int selecting = UnityEngine.Random.Range(0, 3);
        switch (selecting)
        {
            case 0: aiType = CpuType.aggressive; break;
            case 1: aiType = CpuType.balanced; break;
            case 2: aiType = CpuType.defensive; break;
            default: aiType = CpuType.aggressive; break;
        }
        Weapon wp = new Gun(100, 25, 1f, 1, 1, 1);
        status.changeWeapon(wp);
        player.Tower.gameObject.AddComponent<AttackingDetector>();
        player.Tower.GetComponent<AttackingDetector>().brainConnetting(this);
        cpu = new PlayerCPU(anim, player, ag, status, this.transform);
        cpu.findNewTarget();
        ag.speed = 20;
        GetComponentInChildren<BulletSpawner>().INITIALIZING(player);
        player.reciveAnimator(anim);
        initialized = true;
        tm = player.team;
        player.onDead();
        isfrozen = false;
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        BulletSpawner spawner = GetComponentInChildren<BulletSpawner>();
        spawner.INITIALIZING(player);
    }
    #endregion
    #region update
    private void Update()
    {
        gravityEffect();
        ag.enabled = onGround();
        if (isInTheVortex)
        {
            vorterxUpdate();
        }
        if (!initialized) return;
        if (isfrozen)
        {
            froze();
            return;
        }
        if (ag.enabled == false) { return; }
        //AI ipdate
        cpu.UpdateStatus();

    }
    #endregion
    //gravity effect
    public void gravityEffect()
    {
        if (onGround())
        {
            dir.y = 0;
        }
        dir.y += Physics.gravity.y * Time.deltaTime * gravityScale;
        rb.velocity = dir;
    }
    //凝っている
    private void froze()
    {
        frozingTime -= Time.deltaTime;
        ag.SetDestination(transform.position);
        if (frozingTime <= 0)
        {
            isfrozen = false;
            frozingTime = 5;
        }
    }
    
    //tower on taking damage reaction, by ai style
    public void onDamageReaction(Team t, bool isPL)
    {

        switch (aiType)
        {
            case CpuType.aggressive:
                cpu.targetCheckTimer = 20;
                cpu.State = StaticStrings.running;
                cpu.target = giveAttackingTarget(t).Tower.transform;
                player.helperDefenceTower();

                break;
            case CpuType.balanced:
                cpu.targetCheckTimer = 20;
                cpu.State = StaticStrings.running;
                cpu.target = giveAttackingTarget(t).Helper.transform;
                player.helperDefenceTower();
                break;
            case CpuType.defensive:
                cpu.targetCheckTimer = 20;
                cpu.State = StaticStrings.running;
                cpu.target = giveAttackingTarget(t).transform;
                break;
        }

    }
    //ターゲット
    B_Player giveAttackingTarget(Team tm)
    {
        foreach (var p in player.enemyList)
        {
            if (p.getTeam() == tm)
            {
                return p;
            }
        }
        return null;
    }

  //ground check
    public bool onGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
    }
    //攻撃
    public void AIshoot()
    {
        player.BULLETSPAWNER.shot();
    }
    //temを渡す
    public Team getTeam()
    {
        return tm;
    }

    public void frozing()
    {
        if (!isfrozen)
            isfrozen = true;
    }

    //爆弾
    public void spawnBomb()
    {
        WPbullet b = Instantiate(bomb, hand.position, hand.rotation);
        Vector3 dir;
        dir = transform.forward * 50;
        b.gameObject.GetComponent<Rigidbody>().AddForce(dir * 50);
    }
    //ジャンプ
    public void jump()
    {
        rb.AddForce(new Vector3(UnityEngine.Random.Range(-power, power), power - 200, UnityEngine.Random.Range(-power, power)));
    }

    public Transform getHand()
    {
        throw new NotImplementedException();
    }
    #region Tornado
    //竜巻の中での処理
    public void Vortex()
    {
        if (isInTheVortex) return;
        isInTheVortex = true;
        vortexPos = transform.position;
        vortexRotation = transform.rotation;
        if (tornado != null) tornado.Play();
        initialized = false;
        ag.ResetPath();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.mass = 0;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(Vector3.up, ForceMode.Impulse);
        gravityScale = -10;
    }

    void vorterxUpdate()
    {

        transform.Translate(0, vortexPos.y +flyHeight, 0,Space.World);
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
    #endregion
}

public enum CpuType
{
    aggressive,
    balanced,
    defensive
}