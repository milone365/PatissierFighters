using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.Networking;
namespace M
{
    public class M_Robot : NetworkBehaviour
    {
        NavMeshAgent agent;
        Animator anim;
        bool havePiece = false;
        M_Tower tower;
        M_PancakeSpawner spawner;
        Transform target;
        float pettingRange = 10;
        float pettingTime = 3;
        float pettingCount = 3;
        bool isPetting = false;
        bool isDeath = false;
        [SerializeField]
        GameObject pancakePiece=null;
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            spawner = FindObjectOfType<M_PancakeSpawner>();
            tower = GetComponentInParent<M_Tower>();
            GetComponent<M_HealthManager>().onDeath += onDeath;
            if (pancakePiece != null) { pancakePiece.SetActive(false); }
        }

        // Update is called once per frame
        void Update()
        {
            if (isDeath) return;
            makeCake();
            anim.SetFloat(StaticStrings.move, agent.velocity.magnitude);
            
        }
       public void onDeath(bool v)
        {
            if (v==true)
            {
                anim.SetTrigger(StaticStrings.death);
                agent.enabled = false;
                isDeath = true;
            }
            else
            {
                agent.enabled = true;
                isDeath = false;
            }
           
        }
        #region cake
        void findPicies()
        {
            if (spawner.pancakes.Count < 1) return;
            List<Transform> targets = new List<Transform>();
            foreach (var p in spawner.pancakes)
            {
                if (p != null)
                {
                    targets.Add(p);
                }
            }
            targets = targets.OrderBy(c => Vector3.Distance(transform.position, c.position)).ToList();
            target = targets[0];
        }
        void makeCake()
        {

            if (havePiece)
            {
                if (target == null)
                {
                    target = tower.transform;
                }
                agent.SetDestination(target.position);
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance <= pettingRange)
                {
                    agent.SetDestination(transform.position);
                    if (havePiece)
                    {
                        CmdPet();
                        
                    }
                }
            }
            else
            {
                if (target == null)
                {
                    findPicies();
                }
                else
                {
                    if (agent.enabled == false) return;
                    Vector3 pos = new Vector3(target.position.x, transform.position.y, target.position.z);
                    agent.SetDestination(pos);
                }
            }
        }
        
        [Command]
        void CmdPet()
        {
            petting();
            RpcPetting();
        }
        void petting()
        {
            if (!isPetting)
            {
                isPetting = true;
                anim.SetTrigger(StaticStrings.petting);
                if (pancakePiece != null) { pancakePiece.SetActive(false); }

            }
            pettingCount -= Time.deltaTime;
            if (pettingCount <= 0)
            {
                pettingCount = pettingTime;
                havePiece = false;
                target = null;
                isPetting = false;
                tower.Build(1);
            }
        }
        [ClientRpc]
        void RpcPetting()
        {
            if (!isServer)
            {
                petting();
            }
        }
        public void takeMaterial()
        {
            havePiece = true;
            anim.SetTrigger(StaticStrings.pickUp);
            if (pancakePiece != null) { pancakePiece.SetActive(true); }
        }
        #endregion
    }
}


