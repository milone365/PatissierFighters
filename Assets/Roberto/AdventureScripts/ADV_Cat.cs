using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ADV_Cat : ADV_Enemies
{
    [SerializeField]
    Transform startPoint = null;
    [SerializeField]
    Transform UpObject=null;
    [SerializeField]
    Slider hpBar = null;
    Collider player;
    bool attack = false;
    [SerializeField]
    SpriteRenderer mark=null;
    public override void init()
    {
        anim = GetComponentInParent<Animator>();
        
        mark.enabled = false;

    }
    //ダメージ
    public override void interact(Vector3 hitpos)
    {
        if (EffectDirector.instance != null)
        {

            int rnd = Random.Range(1, 3);
            if (rnd == 1)
            {
                effect = StaticStrings.HELPERMELEE;
            }
            else
            {
                effect = StaticStrings.MELEE2;
            }
            EffectDirector.instance.playInPlace(hitpos, effect);
        }
        base.takeDamage();

    }
    //移動
    public override void Updating()
    {

        if (!canmove||target==null) return;  
        targetPos = new Vector3(target.position.x, UpObject.position.y, target.position.z);
        UpObject.position = Vector3.MoveTowards(UpObject.position, targetPos, moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(UpObject.position, targetPos);
        if (distance <= 0.5f)
        {
            FindTarget();
        }
        UpObject.LookAt(targetPos, Vector3.up);
        UpObject.rotation = Quaternion.Euler(0, UpObject.rotation.eulerAngles.y, 0);
        anim.SetFloat(StaticStrings.move, distance);
        if (hpBar != null)
        {
            hpBar.value = hp;
        }
    }
    //プレーヤーへ向かって、ｈｐバーが見えるように。。。
    public override void follow(Transform t)
    {
        mark.enabled = true;
        base.follow(t);
        GetComponent<Collider>().enabled = true;
        if (hpBar == null) return;
        hpBar.gameObject.SetActive(true);
        hpBar.maxValue = hp;
        hpBar.value = hp;
        Invoke("DeactiveMark", 2.5f);
    }

    public void DeactiveMark()
    {
        mark.enabled = false;
    }
    //後ろに戻るため
    public override void comeBack()
    {
        target = startPoint;
    }
    //攻撃
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticStrings.player)
        {
            if (attack) return;
            attack = true;
            player = other;
            anim.SetTrigger("ATK");
            Invoke("getDamageToPlayer", 3);
        }
    }
    //プレーヤーにダメージをあげる
    public void getDamageToPlayer()
    {
        IHealth h = player.GetComponent<IHealth>();
        h.takeDamage(3);
        comeBack();
    }
}
