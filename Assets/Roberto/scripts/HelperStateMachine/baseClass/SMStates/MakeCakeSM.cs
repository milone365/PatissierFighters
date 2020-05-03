using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class MakeCakeSM : AIState
{
    Transform pancakeTarget;
    float pettingrange = 0;
    Vector3 pos =Vector3.zero;
    public MakeCakeSM(NavMeshAgent ag, FiniStateMachine st, KitchenHelper hp, STATUS _status, stateType stateTYPE, Animator an) : base(ag, st, hp, _status, stateTYPE, an)
    {
        
        helper.onReciveMaterial += OnReciveMaterial;
        pettingrange = status.pettingrange;
    }
    bool waiting;
    float waitingTime = 3;
    float waitCounter = 3;
    //タワーとの距離のチェック
    void makeCAKE()
    {
        float distance = Vector3.Distance(helper.transform.position, helper.towerScript.transform.position);
        if (distance <= pettingrange)
        {
            if (agent.destination != agent.transform.position)
            moveTo(helper.transform.position);

            anim.SetTrigger(StaticStrings.petting);
            helper.haveMaterial = false;
            helper.towerScript.Build(helper.pancakeValue);
            waiting = true;
        }
        
        
    }
   
    public override void EnterState()
    {
        base.EnterState();
        stateMachine.WeaponModel.SetActive(false);   
    }
    //マテリアルを貰って、タワーへ移動
    public void OnReciveMaterial()
    {
        anim.SetTrigger(StaticStrings.pickUp);
        moveTo(helper.towerScript.transform.position);
    }

    //material を集りにいく
    public void goToTakeMaterial()
    {
        
        float d = float.MaxValue;
        
        foreach (Transform t in PancakeSpawner.instance.pancakes)
        {
            if (t == null) continue;
            pos = new Vector3(t.position.x, helper.transform.position.y, t.position.z);
            float distanc = Vector3.Distance(helper.transform.position, pos);
            if (distanc < d)
            {
                d = distanc;
                pancakeTarget = t;
            }
        }
        if (pancakeTarget == null) {  return; } 
        moveTo(pos);
    }
   
   //パンケーキを取りに行って、皿へ戻る
    public override void UPDATING()
    {
        if (waiting)
        {
            moveTo(helper.transform.position);
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                waitCounter = waitingTime;
                waiting = false;
            }
            return;
        } 
        if (helper.haveMaterial)
        {
            makeCAKE();
        }
        else
        {
            goToTakeMaterial();
            
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        helper.haveMaterial = false;
    }
} 
 