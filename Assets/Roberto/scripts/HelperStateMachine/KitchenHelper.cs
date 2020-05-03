using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using SA;
public class KitchenHelper : MonoBehaviour,ITeam,IAttack<Transform>
{
    public event Action onReciveMaterial;
   Transform target;
    public Transform TARGET { get { return target; } set { target = value; } }
    Animator anim;
    bool havematerial=false;
    public List<B_Player> enemies = new List<B_Player>();
    Team team;
    public int pancakeValue = 1;
    public PancakeTower towerScript = null;
    B_Player player = null;
    public B_Player getPlayer() { return player; }
    public bool haveMaterial { get { return havematerial; } set { havematerial = value; } }
    public STATUS status;
    
    public Team getTeam()
    {
        return team;
    }

    #region initializing
    public void INIT(B_Player PLAYER,Team TEAM)
    {
        status = new STATUS();
        player = PLAYER;
        team = TEAM;
        anim = GetComponent<Animator>();
      //gameManagerにアクセスして敵のタワーを探す
       foreach (var p in B_GameManager.instance.playerList)
        {
            ITeam team = p.gameObject.GetComponent<ITeam>();
            if (team.getTeam() != this.team)
            {
                enemies.Add(p);
            }
        }
        status.getCurrentweapon.attackdelay = 1.5f;
    }
    #endregion

   //マテリアルを貰って、タワーへ移動
    public void reciveMaterial()
    {
       havematerial = true;
        if(onReciveMaterial!=null)
        onReciveMaterial();

    }
    public void onGiveTarge(Transform t)
    {
        TARGET = t;
    }
   
    //eventターゲットを変える
    public void INITIALIZETARGETEVENT(Inputhandler hand)
    {
        Inputhandler handler = hand;
        if (hand != null)
        {
            
            hand.onGiveTarget += onGiveTarge;
        }
        else
        {
            Debug.Log("Selector is null");
        }
    }

    //攻撃
    public void attack(Transform t)
    {
        IHealth health = t.GetComponent<IHealth>();
        if (health == null)
        {
            Debug.LogError("Target dont'have healthManagement");
            return;
        }
        health.takeDamage(status.getCurrentweapon.Attack + status.ATTACK);
    }
    //スピードと攻撃力アップ
    public void powerUp(float value)
    {
        HealthManager health = GetComponent<HealthManager>();
        health.powerUp(value);
        NavMeshAgent ag = GetComponent<NavMeshAgent>();
        ag.speed ++;
        status.ATTACK ++;
       
    }
    //手
    
    public Transform getHand()
    {
        throw new NotImplementedException();
    }
}
