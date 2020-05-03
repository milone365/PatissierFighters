using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "phase", menuName = "BossPhase/Phase/RugbyEvent")]
public class BossRugbyEvent : BossPhase
{
    [SerializeField]
    float attackDelay = 3;
    float attackTime;
    
    //入る
    public override void ENTERSTATE(ADV_Boss b,Animator a)
    {
        //references
        boss = b;
        anim = a;
       
       　int animationIndex;
        Debug.Log("RugbyPhase");
        //animation
        animationIndex = (int)BossAnimations.rugby;
        anim.SetInteger("PHASE", animationIndex);
        //player ref
        player = b.Get_player();
        attackTime = attackDelay;
        //damegeable parts active
        b.activePhaseParts();
        //update play
        boss.wait = false;
    }
    //時間経ったら攻撃
    public override void UPDATE()
    {
        attackTime -= Time.deltaTime;
        
        if (attackTime <= 0)
        {
            attackTime = attackDelay;
            boss.robotsAttack();
        }
    }
    public override void EXITSTATE()
    {
      
    }
}
