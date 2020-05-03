using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FiniStateMachine : MonoBehaviour,IFroze,ITornado
{
    float flyHeight = 1;
    Vector3 vortexPos;
    Quaternion vortexRotation;
    bool isInTheVortex = false;
    public AIState currentState;
    AIState pastState;
    Dictionary<stateType, AIState> validStates;
    NavMeshAgent agent;
    KitchenHelper helper;
    //internal bool canMove;
    private bool isDead;
    B_Player player;
    Animator anim;
    public GameObject pancakePiece;
    public GameObject WeaponModel;
   float frozingTime = 5;
   public bool isfrozen { get; set;}
    float invincibleCounter = 5;
    [SerializeField]
    ParticleSystem effect = null;
    bool isInvincible=false;
    float vortexTime = 8;
    //bool charge = true;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        helper = GetComponent<KitchenHelper>();
        player = helper.getPlayer();
        WeaponModel.SetActive(false);
        EventsInitializing();
        STATUS status = helper.status;
        //AistateのDictionaryを作る
        validStates = new Dictionary<stateType, AIState>();
        validStates.Add(stateType.idle, new IdleSM(this.agent, this, helper, status, stateType.idle, anim));
        validStates.Add(stateType.attack, new AttackSM(this.agent, this, helper, status, stateType.attack, anim));
        validStates.Add(stateType.defence, new DefenceSM(this.agent, this, helper, status, stateType.defence, anim));
        validStates.Add(stateType.cake, new MakeCakeSM(this.agent, this, helper, status, stateType.cake, anim));
        //現在段階
        currentState = validStates[stateType.defence];
        //元の段階
        pastState = validStates[stateType.cake];
        changeState(stateType.idle);
        isfrozen = false;
    }

   
    void Update()
    {
        if (isInTheVortex)
        {
            vorterxUpdate();
        }
        if (isDead) return;
        if (isfrozen)
        {
            froze();
            return;
        }
        updateAnimator();
        currentState.UPDATING();
        if (isInvincible)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                invincibleCounter = 5;
                becomeInvincible(false);
            }
        }
    }

    //段階を変える
    internal void changeState(stateType statetype)
    {
        pastState = currentState;
        //現在から出る
        currentState.ExitState();
        if (validStates.ContainsKey(statetype))
        {
            currentState = validStates[statetype];
        }
        else
        {
            Debug.LogError("don't have this state assigned");
        }
        //新しいに入る
        if(pastState.stateType != statetype||statetype==stateType.attack)
        {
            currentState.EnterState();

        }
      
    }

    //animator
    void updateAnimator()
    {
        Vector3 velocity = agent.velocity;
        anim.SetFloat(StaticStrings.move, velocity.magnitude);
        anim.SetBool(StaticStrings.HAVEMATERIAL, helper.haveMaterial);
        pancakePiece.SetActive(helper.haveMaterial);
    }

    //event initializations
    void EventsInitializing()
    {
        HealthManager health = GetComponent<HealthManager>();
        if (health != null)
        {
            health.onDeath += onDeath;
        }

        player.onChangeOrder += changeState;
    }
    //死
    public void onDeath(bool value)
    {
        isDead = value;
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger(StaticStrings.death);
    }
    //Collider active/deactive
    public void WeaponCollider(int v)
    {
        bool value;
       if (v == 1)
        {
            value = true;
        }
        else
        {
            value = false;
        }
        WeaponModel.gameObject.GetComponentInChildren<Collider>().enabled = value;
    }


   
    //攻撃アニメーション、ランダムで選ぶ
    public  string randomAttack()
    {
        string atk;
        int num = UnityEngine.Random.Range(1,helper.status.getCurrentweapon.numOfattack);
        switch (num)
        {
            case 1:
                atk = StaticStrings.attack1;
                break;
            case 2:
                atk = StaticStrings.attack2;
                break;
            case 3:
                atk = StaticStrings.attack3;
                break;
            default:
                atk = StaticStrings.attack1;
                break;
        }
        return atk;
    }
    //凝る
    private void froze()
    {
        frozingTime -= Time.deltaTime;
        agent.SetDestination(transform.position);
        if (frozingTime <= 0)
        {
            isfrozen = false;
            frozingTime = 5;
        }
    }

    public void frozing()
    {
        isfrozen = true;
    }
    //無敵になる
    public void becomeInvincible(bool v)
    {
        if (effect == null) return;
        isInvincible = v;
        if (v)
            effect.Play();
        else
            effect.Stop();

        HealthManager h = GetComponent<HealthManager>();
        if (h)
        {
            h.becameInvincible(v);
        }
    }

    //竜巻処理
    public void Vortex()
    {
        if (isInTheVortex) return;
        isInTheVortex = true;
        vortexPos = transform.position;
        vortexRotation = transform.rotation;
        if (effect != null) effect.Play();
        isDead = true;
        agent.ResetPath();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.mass = 0;
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(Vector3.up, ForceMode.Impulse);
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
            rb.constraints = RigidbodyConstraints.FreezeAll;
            isDead = false;
            transform.rotation = vortexRotation;
            transform.position = vortexPos;
            if (effect != null) effect.Stop();
            flyHeight = 1;
        }
    }
}
