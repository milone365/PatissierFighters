using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using M;
namespace td
{
    public class M_TD_PancakeBall : NetworkBehaviour
    {
        public event Action<Team,Transform> onTakingBall;
        bool isTaked = false;
        Collider c;
        public bool getIsTaked()
        {
            return isTaked;
        }
       Team possessorTm = Team.chocolate;
        public Team getPossessor()
        {
            return possessorTm;
        }
        public static M_TD_PancakeBall instance;
        [SerializeField]
        ParticleSystem particle=null;
        MU_TD_GameManager gm;
        Vector3 startPosition=Vector3.zero;
       
       
        private void Awake()
        {
            instance = this;
            gm = FindObjectOfType<MU_TD_GameManager>();
            startPosition = transform.position;
            gm.setResetableObjects(this.gameObject);

        }
        Transform hand;

        public bool canMove { get ; set ; }

     
        #region reset
        [Command]
        void CmdReset()
        {
            rst();
            RpcReset();
        }
        [ClientRpc]
        void RpcReset()
        {
            if (!isServer)
            {
                rst();
            }
        }
        void rst()
        {

            transform.SetParent(null);
            transform.rotation = new Quaternion(0, 0, 0, 0);
            if (particle != null)
            {
                particle.Play();
            }
            transform.position = startPosition;
            isTaked = false;
        }
        public void resetStatus()
        {
            CmdReset();
        }
        #endregion
        
        #region Lose

        public void loseBall()
        {
            CmdLoseBall();
            RpcclienteLoseBall();
        }
        [Command]
        void CmdLoseBall()
        {
            StartCoroutine(loseballCo());
        }
        [ClientRpc]
        void RpcclienteLoseBall()
        {
            if (!isServer)
            {
                StartCoroutine(loseballCo());
            }
        }
        IEnumerator loseballCo()
        {
            GetComponent<Collider>().enabled = false;
            transform.SetParent(null);
            if (particle != null)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(1f);
            GetComponent<Collider>().enabled = true;
            isTaked = false;

        }

        #endregion


        #region takeBll
        private void OnTriggerEnter(Collider other)
        {
           if(isTaked) return;
            c = other;
            CmdTakeBall();
        }
        void takeTheball(Collider other)
        {
            
            if (other.tag == StaticStrings.AI || other.tag == StaticStrings.player
                || other.tag == StaticStrings.helper || other.tag == StaticStrings.cpu)
            {
               
                M_TD_Character c = other.GetComponent<M_TD_Character>();
                if (c != null)
                {
                    isTaked = true;
                    if (onTakingBall != null)
                    {
                        onTakingBall(possessorTm, other.transform);

                    }
                    c.haveBall(true);
                    takeIn_Hand(c.getHand());
                   
                }
                
                ITeam team = other.GetComponent<ITeam>();
                possessorTm = team.getTeam();
            }
        }
        void takeIn_Hand(Transform t)
        {
            transform.SetParent(t);
            transform.localPosition = new Vector3(0, 0, 0);

            if (particle != null)
            {
                particle.Stop();
            }
        }
        [Command]
        void CmdTakeBall()
        {
            takeTheball(c);
            RpctakeTheball();
        }
        [ClientRpc]
        void RpctakeTheball()
        {
            if (!isServer)
            {
                takeTheball(c);
            }
        }
        #endregion
    }

}
