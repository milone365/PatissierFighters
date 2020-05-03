using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SA;

namespace td
{
    //Input handler に基づいて作られたスクリプト
    public class TD_InputHandler : MonoBehaviour
    {
        string playerId="P1_";
        public string playerID { get { return playerId; } set { playerId = value; } }
        float horizontal;
        float vertical;
        public event Action<Transform> onGiveTarget;
        bool isInit;
        B_Player player;
        float delta;
        public TD_StateManager states;
        TD_CameraHandler camerahandler;
        Animator anim;
        TD_canvas canvas = null;

        float fireRate = 0.3f;
        float fireCounter = 0.3f;
        public bool isfrozen { get; set; }

        bool square_input()
        {
            return Input.GetButtonDown(playerID + StaticStrings.Square_key);
        }
        bool L2Input()
        {
            return Input.GetButton(playerID + StaticStrings.L2_key);

        }
        bool R2Input()
        {

            return Input.GetButton(playerID + StaticStrings.R2_key);
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            //dependency with statemanager
            states.init();
            camerahandler = FindObjectOfType<TD_CameraHandler>();
            camerahandler.Init(this);
            isInit = true;
        }
      
        #region Fixed Update

        void FixedUpdate()
        {
            if (!isInit)
                return;
            delta = Time.fixedDeltaTime;
            GetInput_FixedUpdate();
            InGame_UpdateStates_FixedUpdate();
            states.FixedTick(delta);
            camerahandler.fixedTick(delta);
            canvas = GetComponentInChildren<TD_canvas>();
        }

        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis(playerID+ StaticStrings.Vertical);
            horizontal = Input.GetAxis(playerID + StaticStrings.Horizontal);

        }

        void InGame_UpdateStates_FixedUpdate()
        {
            states.inp.horizontal = horizontal;
            states.inp.vertical = vertical;

            states.inp.moveAmount = (Mathf.Clamp01(Mathf.Abs(horizontal)) + Mathf.Abs(vertical));

            Vector3 moveDir = camerahandler.mTransform.forward * vertical;
            moveDir += camerahandler.mTransform.right * horizontal;
            moveDir.Normalize();
            states.inp.moveDirection = moveDir;
            states.inp.rotateDirection = camerahandler.mTransform.forward;
            
        }
        #endregion
        #region Update
        void Update()
        {
            if (!isInit)
                return;
            
            delta = Time.deltaTime;
            states.tick(delta);
            crossAirUpdate();
             if (R2Input())
            {
                fire();
            }
            if (square_input())
            {
                tossBomb();
            }
        }

        private void tossBomb()
        {
            anim.SetTrigger(StaticStrings.bomb);
        }

        void crossAirUpdate()
        {
            if (!states.onGround()) return;
            states.isAiming = L2Input();
            if (canvas == null) return;
            canvas.activeCrossAir(L2Input());
        }
        #endregion

        void fire()
        {
            fireRate -= Time.deltaTime;
            if (fireRate <= 0)
            {
                fireRate = fireCounter;
                Target_and_Shoot();
            }

        }
        void Target_and_Shoot()
        {

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(StaticStrings.shooting) && !anim.IsInTransition(0))
                anim.SetTrigger(StaticStrings.shooting);
        }


        public void reciveTrget(Transform t)
        {
            if (onGiveTarget != null)
            {
                onGiveTarget(t);
            }
        }

        public void frozing()
        {
            isfrozen = true;
        }
    }
}


