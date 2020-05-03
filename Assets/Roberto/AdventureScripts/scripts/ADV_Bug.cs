using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADV_Bug : ADV_Enemies
{
  
    //delegate
    delegate void intercatAction();
    intercatAction currentAction;
    
    bool cameBack=false;
    [SerializeField]
    float bulletSpeed = 50;
    Vector3 headpos=Vector3.zero;
 
    public override void init()
    {
        anim = GetComponent<Animator>();
        currentAction = takeDamage;
        

    }
    public override void interact(Vector3 hitpos)
    {
        currentAction();
    }

    public override void Updating()
    {
        if (cameBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, headpos, bulletSpeed * Time.deltaTime);
            return;
        }
        base.Updating();
    }
    //
    public override void takeDamage()
    {
        if (isDead) return;
        hp--;
        if (hp <= 0)
        {
            isDead = true;
            anim.SetTrigger(StaticStrings.death);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.generatePopUp(scorepoints);
            }
            //関数を変える
            currentAction = becameProjectile;
        }
    }
    //ボースの頭
    public void GiveHead(Transform t)
    {
       headpos = new Vector3(t.position.x, t.position.y + 10, t.position.z);
    }
    //1後ろに戻る
    void becameProjectile()
    {
        cameBack = true;
    }

    //damage 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            IHealth h = other.GetComponent<IHealth>();
            if (h == null) return;
            h.takeDamage(1);
        }
        if (!cameBack) return;
        if (other.tag == "Boss")
        {
            IEnemy h = other.GetComponent<IEnemy>();
            h.takeDamage(10);
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.blood);
            }
            Destroy(gameObject);
        }
    }
}
