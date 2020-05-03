using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "phase", menuName = "BossPhase/Phase/BattleShip")]
public class BossEndPhase : BossPhase
{
    Transform head;
    
    //わからなかったら他のphaseを見て下さい
    public override void ENTERSTATE(ADV_Boss b,Animator a)
    {
        boss = b;
        anim = a;
        int animationIndex;
        animationIndex = (int)BossAnimations.battleShip;
        anim.SetInteger("PHASE", animationIndex);
        player = b.Get_player();
        attackCounter = 3;
        boss.wait = false;
         head = boss.getHead();
        head.GetComponent<Collider>().enabled = true;
        head.tag = "Boss";
        head.GetComponent<BossPart>().enabled = true;
        b.activePhaseParts();


    }
    public override void UPDATE()
    {
    }
    public override void EXITSTATE()
    {
        
    }
}
