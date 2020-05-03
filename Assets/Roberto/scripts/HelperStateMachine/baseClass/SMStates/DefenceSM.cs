using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DefenceSM : AIState
{
    Transform enemyTarget;
    float chasingDistance, attackingTime;
    float attackRange, attacktimer;
    public float towerRange=8;
    public DefenceSM(NavMeshAgent ag, FiniStateMachine st, KitchenHelper hp, STATUS _status, stateType stateTYPE, Animator an) : base(ag, st, hp, _status, stateTYPE,an)
    {
        chasingDistance = status.getCurrentweapon.chasingdistance;
        attackingTime = status.getCurrentweapon.attackdelay;
        attackRange = status.getCurrentweapon.attackrange;
        attacktimer = attackingTime;
    }
  
    public override void UPDATING()
    {
        if (attacking())
        {
            attack(enemyTarget);
        }
        else
        {
            defence();
        }
       
    }
    public override void EnterState()
    {
        base.EnterState();
        Preparedefence();
        stateMachine.WeaponModel.SetActive(true);
    }
    public override void ExitState()
    {
        base.ExitState();
        stateMachine.WeaponModel.SetActive(false);
    }
    void Preparedefence()
    {
       if(enemyTarget!= null){
            enemyTarget = null;
        }
       moveTo(helper.towerScript.transform.position);
    }
    //タワーへ移動して、止まる。
    void defence()
    {
        if (reachPoint(0.8f))
        {

            moveTo(helper.transform.position);

        }
        //確認、敵が近づく確認
        RaycastHit[] hits = Physics.SphereCastAll(helper.transform.position, chasingDistance, Vector3.up, default);

        foreach (var hit in hits)
        {

            Transform target = hit.collider.transform;
            if (target.GetComponent<ITeam>() == null)
                return;
            ITeam tm = target.GetComponent<ITeam>();

            if (tm.getTeam() != helper.getTeam())
            {
                enemyTarget = target;
                
            }
        }
    }
    //攻撃
    public void attack(Transform enemy)
    {
        float distance = Vector3.Distance(helper.transform.position, enemyTarget.position);
       
        if (distance <= attackRange)
        {
            if (agent.destination != helper.transform.position)
               moveTo(helper.transform.position);

            attacktimer -= Time.deltaTime;
            if (attacktimer <= 0)
            {
                attacktimer = attackingTime;
               if (!anim.IsInTransition(1))
                    anim.SetTrigger(stateMachine.randomAttack());
            }
        }
        else if (distance > attackRange && distance <= chasingDistance)
        {
            moveTo(enemyTarget.position);
        }
        else if (distance > chasingDistance||!isIntowerRange() )
        {
            enemyTarget = null;
        }

    }
    //敵は近いかどうかの確認
    bool isIntowerRange()
    {
        float towerDistance = Vector3.Distance(helper.transform.position, helper.towerScript.transform.position);
        return towerDistance <= towerRange;
       
    }
    bool attacking()
    {
        return enemyTarget != null;
    }
}
