using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "phase", menuName = "BossPhase/Phase/Bombs")]
public class BossPhaseBombs : BossPhase
{
  
    [SerializeField]
    float spawningTime = 2;
    float spawningCounter=10;
    const float counter = 6;


    public override void ENTERSTATE(ADV_Boss b,Animator a)
    {
        boss = b;
        transform = b.transform;
        anim = a;
        int animationIndex;
        animationIndex = (int)BossAnimations.ground;
        anim.SetInteger("PHASE", animationIndex);
        player = b.Get_player();
        //fadescreenを消す
        GameObject fadeScreen;
        fadeScreen = GameObject.Find("FadeScreen");
        if (fadeScreen != null)
        {
            fadeScreen.GetComponent<Animation>().Play("unfade");
        }
        spawningCounter = counter;
        boss.wait = false;
        b.activePhaseParts();
    }

    public override void UPDATE()
    {
        spawningCounter -= Time.deltaTime;
        if (spawningCounter <= 0)
        {
            spawningCounter = spawningTime;
            boss.spawnEnemy();
        }
    }
    public override void EXITSTATE()
    {
       
    }
}
