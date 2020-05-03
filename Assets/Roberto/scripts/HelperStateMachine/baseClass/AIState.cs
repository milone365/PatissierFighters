using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//BASE CLASS
public class AIState 
{
   protected FiniStateMachine stateMachine { get; set; }
   protected NavMeshAgent agent { get; set;}
   protected KitchenHelper helper;
   protected STATUS status;
   public stateType stateType;
   protected Animator anim;

    public AIState(NavMeshAgent ag, FiniStateMachine st, KitchenHelper hp, STATUS _status, stateType stateTYPE,Animator an)
    {
        stateMachine = st;
        agent = ag;
        helper = hp;
        status = _status;
        stateType = stateTYPE;
        anim = an;
    }

 public virtual void EnterState() { agent.ResetPath();}

    public virtual void UPDATING() { }

    public virtual void ExitState()
    {
        agent.SetDestination(helper.transform.position);
    }

    protected bool reachPoint(float valuer)
    {
        return agent.remainingDistance <= valuer;
    }

    public void moveTo(Vector3 pos)
    {

        agent.SetDestination(pos);
    }

    public virtual void onReciveTarget(Transform t) { }
}

public enum stateType
{
    idle,
    attack,
    defence,
    cake
}