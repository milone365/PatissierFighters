using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "phase", menuName = "BossPhase/Phase/Bugs")]
public class BossBugEvent : BossPhase
{
    [SerializeField]
    float spawningTime = 2;
    float spawningCounter = 10;
    const float counter = 6;
    [SerializeField]
    GameObject bug=null;
    public override void ENTERSTATE(ADV_Boss b,Animator a)
    {
        boss = b;
        transform = b.transform;
        anim = a;
        
        int animationIndex;
        animationIndex = (int)BossAnimations.phase3;
        anim.SetInteger("PHASE", animationIndex);
        player = b.Get_player();
        b.addElentToList(b.middlePortal);
        //敵を変える
        b.changeEnemyToSpawn(bug);
        spawningCounter = counter;
        b.deactiveCamera();
        boss.wait = false;
        b.getHead().GetComponent<BossPart>().enabled = true;
        b.activePhaseParts();
    }
    public override void UPDATE()
    {
        spawningCounter -= Time.deltaTime;
        if (spawningCounter <= 0)
        {
            spawningCounter = spawningTime;
            boss.spawnBug();
        }
    }
    public override void EXITSTATE()
    {
        
    }
}
