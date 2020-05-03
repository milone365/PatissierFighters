using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace td
{
    public class TD_PlayerControler : TD_Character
    {
        TD_Ally ally;
        #region returnValues
       
        public TD_Ally getAlly()
        {
            return ally;
        }

        public void resetStatus()
        {
            respawning();
            canMove = true;
        }
   
   
        #endregion


        #region initialing  
      
        public override void Init()
        {
            TD_GameManager gm = FindObjectOfType<TD_GameManager>();
            gm.setResetableObjects(this.gameObject);
            ally = FindObjectOfType<TD_Ally>();
            GameObject door = GameObject.FindGameObjectWithTag("door");
            door.GetComponent<TD_PointZone>().tm = this.team;
             base.Init();
        }
        #endregion

        public override void updating()
        {
            base.updating();
            
        }

    }
}

