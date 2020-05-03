using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace td
{
    public class TD_PancakeBall : MonoBehaviour
    {
        public event Action<Team,Transform> onTakingBall;
        bool isTaked = false;
        public static TD_PancakeBall instance;
        public bool getIsTaked()
        {
            return isTaked;
        }
       Team possessorTm = Team.chocolate;
        public Team getPossessor()
        {
            return possessorTm;
        }
       
        [SerializeField]
        ParticleSystem particle=null;
        TD_GameManager gm;
        Vector3 startPosition=Vector3.zero;
       
       
        private void Awake()
        {
            instance = this;
            gm = FindObjectOfType<TD_GameManager>();
            startPosition = transform.position;
            gm.setResetableObjects(this.gameObject);

        }
        Transform hand;

        public bool canMove { get ; set ; }

        #region addPoint and reset
        public void givePoint()
        {
            gm.addPointToPlayer(possessorTm);
        }

        public void resetStatus()
        {
            transform.SetParent(null);
            transform.rotation = new Quaternion(0,0,0,0);
            if (particle != null)
            {
                particle.Play();
            }
            isTaked = false;
            transform.position = startPosition;
        }
        #endregion
        #region takeAndLose

        public void loseBall()
        {
            StartCoroutine(loseballCo());
        }

        IEnumerator loseballCo()
        {
            transform.SetParent(null);
            GetComponent<Collider>().enabled = false;
            if (particle != null)
            {
                particle.Play();
            }
            yield return new WaitForSeconds(1f);
            isTaked = false;
            GetComponent<Collider>().enabled = true;
            
        }
        private void OnTriggerEnter(Collider other)
        {
            if (isTaked) return;
            if (other.tag == StaticStrings.AI||other.tag==StaticStrings.player
                ||other.tag==StaticStrings.helper||other.tag==StaticStrings.cpu)
            {
                TD_Character c = other.GetComponent<TD_Character>();
                if (c != null)
                {
                    isTaked = true;
                    c.haveBall(true);
                    possessorTm = c.team;
                    gm.GivePossessor(c.team);
                    takeIn_Hand(c.getHand());
                     if (onTakingBall != null)
                    {
                        onTakingBall(possessorTm, other.transform);

                    }
                    else
                    {
                        Debug.Log("on taking ball is null");
                    }
                }
            }
        }
        #endregion

        void takeIn_Hand(Transform t)
        {
            transform.SetParent(t);
            transform.localPosition = new Vector3(0, 0, 0);
            
            if (particle != null)
            {
                particle.Stop();
            } 
        }

     
    }

}
