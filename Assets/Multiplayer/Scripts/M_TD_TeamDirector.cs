using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace td
{
    public class M_TD_TeamDirector : MonoBehaviour
    {
        public Team team;
        public Letter letterTm;
        MU_TD_GameManager gm;
        [SerializeField]
        Transform door = null;
        List<GameObject> characterList = new List<GameObject>();
        List<M_TD_Ally> robotList = new List<M_TD_Ally>();
        List<M_TD_Character> enemylist = new List<M_TD_Character>();
        public void Init(MU_TD_GameManager gameMan)
        {
            gm = gameMan;
            if (letterTm == Letter.A)
            {
                gm.playerA.t = team;
                gm.playerA.door = door;
            }
            else
            {
                gm.playerB.t = team;
                gm.playerB.door = door;
            }
            M_TD_Character[] allcharacter = FindObjectsOfType<M_TD_Character>();
            foreach(var item in allcharacter)
            {
                if (item.team == this.team)
                {
                    characterList.Add(item.gameObject);
                    M_TD_Ally ally = item.GetComponent<M_TD_Ally>();
                    if (ally != null)
                    {
                        robotList.Add(ally);
                    }
                }
                else
                {
                    enemylist.Add(item);
                }
                
            }
            
            M_TD_PancakeBall.instance.onTakingBall += onTakingBallReaction;
           
        }

      
        public void addCharacter(M_TD_Character c)
        {
            characterList.Add(c.gameObject);
        }
        public void onTakingBallReaction(Team tm,Transform t)
        {
            if (tm != this.team)
            {
                foreach(var r in robotList)
                {
                    if (r.currentAction != action.recover)
                    {
                        r.attackTarget(t);
                    }
                 
                }
            }
            else
            {
                foreach (var r in robotList)
                {
                    if (!r.getBall()&& r.currentAction == action.recover)
                    {
                        int rnd = Random.Range(0, enemylist.Count - 1);
                        r.attackTarget(enemylist[rnd].transform); 
                    }
                   
                }
            }
        }
    }

   
    public enum Letter
    {
        A,
        B
    }
}

