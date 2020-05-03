using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace td
{
    public class TD_TeamDirector : MonoBehaviour
    {
        public Team team;
        public Letter letterTm;
        TD_GameManager gm;
        [SerializeField]
        Transform door = null;
　　　　 List<TD_Ally> robotList = new List<TD_Ally>();
        List<TD_Character> enemylist = new List<TD_Character>();
        TD_PancakeBall pancakeball;
        
        //チームとゴールラインを決める
        public void Init(TD_GameManager gameMan)
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
            
            TD_Character[] allcharacter = FindObjectsOfType<TD_Character>();
            foreach(var item in allcharacter)
            {
                if (item.team == this.team)
                {
                    TD_Ally ally = item.GetComponent<TD_Ally>();
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
            pancakeball = FindObjectOfType<TD_PancakeBall>();
            //イベントの設定
            pancakeball.onTakingBall += onTakingBallReaction;
           
        }
        //ボールが取られたに対しての反応
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
                    //攻撃の命令
                    if (!r.getBall()&& r.currentAction == action.recover)
                    {
                        int rnd = Random.Range(0, enemylist.Count - 1);
                        r.attackTarget(enemylist[rnd].transform); 
                    }
                   
                }
            }
        }
    }

}

