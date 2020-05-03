using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;


public class AttackSM : AIState
{
   float attacktimer;
   float attackingTime;
   PancakeTower tower;
   HealthManager enemyHealt;
   //float attackRange = 1;
    Vector3 pos;
    public AttackSM(NavMeshAgent ag, FiniStateMachine st, KitchenHelper hp, STATUS _status, stateType stateTYPE, Animator an) : base(ag, st, hp, _status, stateTYPE, an)
    {
        attackingTime = status.getCurrentweapon.attackdelay;
        attacktimer = attackingTime;
       
    }
   
    public override void EnterState()
    {
        base.EnterState();
       stateMachine.WeaponModel.SetActive(true);
        if (helper.TARGET == null)
        {
            Debug.LogError("helper don't have target");
            return;
        }
        moveTo(helper.TARGET.position);
        
    }
    public override void UPDATING()
    {
       reachAndAttack();
       
    }

    #region attack

    //攻撃
    void reachAndAttack()
    {
        float distance = Vector3.Distance(helper.TARGET.position, helper.transform.position);
        helper.transform.LookAt(helper.TARGET.position,Vector3.up);
        helper.transform.rotation = Quaternion.Euler(0, helper.transform.eulerAngles.y, 0);
        if (distance<= status.getCurrentweapon.attackrange)
        {
            if (agent.destination != helper.transform.position)
                agent.SetDestination(helper.transform.position);
            //animation
            attacktimer -= Time.deltaTime;
            if (attacktimer <= 0)
            {
                attacktimer = status.getCurrentweapon.attackdelay;
                if (!anim.IsInTransition(1))
                    anim.SetTrigger(stateMachine.randomAttack());

            }

        }
       
        if (distance > status.getCurrentweapon.attackrange)
        {
            moveTo(helper.TARGET.transform.position);
        }
         
        
    }
    #endregion
    public override void ExitState()
    {
        base.ExitState();
        stateMachine.WeaponModel.SetActive(false);
    }
    
    
}
