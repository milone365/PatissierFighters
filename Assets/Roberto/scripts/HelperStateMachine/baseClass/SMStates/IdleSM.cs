using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IdleSM :AIState
{
    [SerializeField]
    float idleDuration = 5;
    float totalDuration;

    public IdleSM(NavMeshAgent ag, FiniStateMachine st, KitchenHelper hp, STATUS _status, stateType stateTYPE, Animator an) : base(ag, st, hp, _status, stateTYPE, an)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        totalDuration = 0;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    //とまって、段階を変える
    public override void UPDATING()
    {
        totalDuration += Time.deltaTime;
        if (totalDuration >= idleDuration)
        {
            stateMachine.changeState(stateType.cake);
        }
    }
}
