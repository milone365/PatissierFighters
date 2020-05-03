using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace td
{
    public class TD_Ally : TD_Character
    {
        Vector3 startPos;
        NavMeshAgent agent;
        Transform pancake;
        Transform enemyDoor;
        Transform target = null;
        Vector3 targetpos = Vector3.zero;
        
       public TD_Status status = null;
        float attackCounter = 0, attackDelay = 0;
        public action currentAction = action.recover;
        action startingAction = action.recover;
        Collider weaponCollider = null;
        public override void Init()
        {
            base.Init();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            startPos = transform.position;
            pancake = TD_PancakeBall.instance.transform;
            attackCounter = status.attackingCounter;
            attackDelay = status.attackingTime;
            GameObject melee = GetComponentInChildren<TD_Attack_melee>().gameObject;
            weaponCollider = melee.GetComponent<Collider>();
            Invoke("StartGame", 2.5f);
            Invoke("FindDoor", 3);
            
        }
        //ゴールラインを探す
        void FindDoor()
        {
            TD_GameManager man = FindObjectOfType<TD_GameManager>();
            man.setResetableObjects(this.gameObject);
            if (team != man.playerA.t)
            {
                enemyDoor = man.playerA.door;
            }
            else
            {
                enemyDoor = man.playerB.door;
            }
        }
        public void StartGame()
        {
            canMove = true;
        }
        //リセット
        public override void reset()
        {
            base.reset();
            currentAction = startingAction;
        }
        public override void updating()
        {
            base.updating();
            if (!canMove) return;
            switch (currentAction)
            {
                case action.recover:
                    Vector3 pancakePos = new Vector3(pancake.position.x, transform.position.y, pancake.position.z);
                    agent.SetDestination(pancakePos);
                    break;
                case action.goUp:
                    Vector3 po = new Vector3(enemyDoor.position.x, transform.position.y, enemyDoor.position.z);
                    agent.SetDestination(po);
                    break;
                case action.chase:
                    if (target == null) return;
                    agent.SetDestination(targetpos);
                    float distance = Vector3.Distance(transform.position, targetpos);
                    if (distance <= status.attackRange)
                    {
                        changeAction(action.attack);
                    }
                    break;
                case action.attack:
                    if (agent.destination != transform.position)
                        agent.SetDestination(transform.position);
                    attack();
                    break;

                case action.none:
                    if (agent.destination != transform.position)
                        agent.SetDestination(transform.position);

                    break;

            }

            anim.SetFloat(StaticStrings.move, agent.velocity.magnitude);
            if (target == null) return;
            targetpos = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
       
        //ゴールラインへ走る
        public void TakingBall()
        {
            changeAction(action.goUp);
        }
        public override void haveBall(bool v)
        {
            base.haveBall(v);
            TakingBall();
        }

        public void changeAction(action a)
        {
            currentAction = a;
        }
        //ターゲットをもたって、敵をフォロー
        public void attackTarget(Transform t)
        {
            target = t;
            changeAction(action.chase);
        }
        public override void respawning()
        {
            agent.ResetPath();
            transform.position = startPos;
            currentAction = startingAction;
        }
        #region atk
        //コライダーを活性するため
        public void WeaponCollider(int num)
        {
            if (num > 0)
            {
                weaponCollider.enabled = true;
            }
            else
            {
                weaponCollider.enabled = false;
            }
        }
        //攻撃
       void attack()
        {
            attackCounter -= Time.deltaTime;
            if (attackCounter <= 0)
            {
                attackCounter = attackDelay;
                anim.SetTrigger(StaticStrings.attack1);
            }
            float distance = Vector3.Distance(transform.position, targetpos);
            if (distance > status.attackRange)
            {
                changeAction(action.chase);
            }
        }
        #endregion
    }

}

