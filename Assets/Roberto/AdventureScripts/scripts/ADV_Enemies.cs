using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Enemies : MonoBehaviour,IShotable, IFollow
{
    [SerializeField]
    protected int scorepoints = 50;
    public TargetFind targetResource = TargetFind.none;
    public EndBattleTipe enemyTipe=EndBattleTipe.destroy;
    protected Transform target;
    protected Vector3 targetPos;
    [SerializeField]
    protected float moveSpeed = 10;
    protected bool canmove = false;
    protected Animator anim;
    protected bool isDead = false;
    [SerializeField]
    protected float hp = 3;
    Vector3 startPosition=Vector3.zero;
    public string effect="";
    [SerializeField]
    protected float attackRange = 1;
    [SerializeField]
    float attackTime = 2;
    float attackCounter = 2;
    public bool movingcharacter = true;
    private void Start()
    {
        init();
    }
    private void Update()
    {
        Updating();
    }
    public virtual void init() {
        anim = GetComponent<Animator>();
        if (targetResource == TargetFind.automatic)
        {
            ADV_Player player = FindObjectOfType<ADV_Player>();
            if (player != null)
            {
                target = player.transform;
                follow(target);
            }
        }
    }
   //移動と攻撃
    public virtual void Updating()
    {
        if (!canmove||target==null||isDead) return;

        targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance <= attackRange)
        {
            anim.SetFloat(StaticStrings.move, 0);
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0)
            {
                attackCounter = attackTime;
                anim.SetTrigger(StaticStrings.attack);
                HurtPlayer();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if(movingcharacter)
            anim.SetFloat(StaticStrings.move, distance);
        }
        transform.LookAt(targetPos,Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        
    }
   //ターゲットのポジションを探す
    public virtual void FindTarget()
    {
        targetPos = new Vector3(targetPos.x, transform.position.y, targetPos.z + 500);
    }
    public virtual void interact(Vector3 hitpos)
    {
        takeDamage();
    }
    //targetを貰う
    public virtual void follow(Transform t)
    {
        target = t;
        canmove = true;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject, 5);
    }
    public virtual void takeDamage()
    {
        if (isDead) return;
        hp--;
        if (hp <= 0)
        {
            isDead = true;
            endBattle();
        }
    }

    void endBattle()
    {
        switch (enemyTipe)
        {
            case EndBattleTipe.destroy:
                Destroy(gameObject);
                break;
            case EndBattleTipe.cameBack:
                comeBack();
                break;
            case EndBattleTipe.deactive:
                gameObject.SetActive(false);
                break;
        }
        if (EffectDirector.instance != null)
        {
            EffectDirector.instance.playInPlace(transform.position, effect);
            EffectDirector.instance.generatePopUp(scorepoints);
        }
}
    //後ろに戻る
    public virtual void comeBack()
    {
        targetPos = startPosition;
    }
    public enum EndBattleTipe
    {
        destroy,
        cameBack,
        deactive
    }
    public void HurtPlayer()
    {

        IHealth health = target.GetComponent<IHealth>();
        if (health != null)
        {
            health.takeDamage(1);
        }
    }
    public enum TargetFind
    {
        automatic,
        none
    }
}

