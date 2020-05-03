using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace td
{
    public class M_TD_PointZone : NetworkBehaviour
    {
        public Team tm;
        Team possessor;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "pancake")
            {
                possessor = M_TD_PancakeBall.instance.getPossessor();
                if (possessor != this.tm)
                {
                    givePoint(possessor);
                }
               
            }
        }

        void givePoint(Team tm)
        {
           MU_TD_GameManager gm = FindObjectOfType<MU_TD_GameManager>();
            gm.addPointToPlayer(tm);
            
        }
  

    }
 
    
}
