using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using SA;

namespace td
{
    public class M_InputHandler : NetworkBehaviour,Imana,IFroze,Iflour
    {
        ParticleSystem electric;
        public GameObject specialBullet;
        skillClass mySkill;
        M_Camera mcamera;
        float horizontal;
        float vertical;
        public event Action<Transform> onGiveTarget;
        public bool isInit;
        B_Player player;
        float delta;
        public M_StateManager states;
        Camera cam;
        Camera Getcamera()
        {
            return cam;
        }
        [SerializeField]
        Transform cameraHolder=null;
        public Transform target;
        Transform pivot;
        Transform cameraHandler;
        public bool leftPivot;
        float delt;
        float mouseX, mouseY, smothx, smothY, smotxVelocity, smothyVelocity;
        float lookAngle, tiltAngle;
        public Cameravalues values;
        Animator anim;
        M_TD_canvas canvas = null;
        M_Camera cm;
        public bool isfrozen { get; set; }
       

        M_BulletSpawner bulletSpawner;
        
       
        bool L2Input()
        {
            return Input.GetButton(StaticStrings.L2_key);

        }
       
        bool triagle()
        {
            return Input.GetButtonDown(StaticStrings.Triangle_key);
        }

        private void Start()
        {
             cm = FindObjectOfType<M_Camera>();
            anim = GetComponent<Animator>();
            //dependency with statemanager
            states.init();
            cam = cm.GetComponentInChildren<Camera>();
            cameraHandler = cm.cameraHandler;
            pivot = cm.cameraPivot;
            target = states.getTransform();
            
            canvas = FindObjectOfType<M_TD_canvas>();
            mySkill = new skillClass();
            electric = GetComponentInChildren<ParticleSystem>();
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
            delt = Time.fixedDeltaTime;
            if (target == null) return;
            handlePosition();
            HandleRotation();
            float speed = values.moveSpeed;
            if (states.isAiming) { speed = values.aimSpeed; }
            Vector3 targetPos = Vector3.Lerp(cameraHandler.transform.position, target.position, delt * speed);
            cameraHandler.transform.position = target.position;
            

        }
 
        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis(StaticStrings.Vertical);
            horizontal = Input.GetAxis(StaticStrings.Horizontal);

        }

        void InGame_UpdateStates_FixedUpdate()
        {
            states.inp.horizontal = horizontal;
            states.inp.vertical = vertical;

            states.inp.moveAmount = (Mathf.Clamp01(Mathf.Abs(horizontal)) + Mathf.Abs(vertical));

            Vector3 moveDir = cameraHandler.forward * vertical;
            moveDir += cameraHandler.right * horizontal;
            moveDir.Normalize();
            states.inp.moveDirection = moveDir;
            states.inp.rotateDirection = cameraHandler.forward;
            
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
             
           
            if (triagle())
            {
                if (mySkill.skillIsActive||mySkill.manaPoints<100) return;
                mySkill.activeSkill();
                GetComponent<M_BulletSpawner>().CmdChangeBullet(specialBullet.name);
                if(electric!=null)
                electric.Play();
            }
            mySkill.Tick(Time.deltaTime);
        }

     

        void crossAirUpdate()
        {
            if (!states.onGround()) return;
            states.isAiming = L2Input();
            if (canvas == null) return;
            canvas.activeCrossAir(L2Input());
        }
        #endregion

       

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
   
        void handlePosition()
        {
            float targetX = values.normalX;
            float targeY = values.normalY;
            float targetZ = values.normalZ;

            if (states.isAiming)
            {
                targetX = values.aimX;
                targetZ = values.aimZ;
            }
            if (leftPivot)
            {
                targetX = -targetX;
            }
            Vector3 newPivotPos = pivot.localPosition;
            newPivotPos.x = targetX;
            newPivotPos.z = targeY;
            Vector3 newCameraPos = cameraHolder.localPosition;
            newCameraPos.z = targetZ;
            float t = delt * values.adaptSpeed;
            pivot.localPosition = Vector3.Lerp(pivot.localPosition, newPivotPos, t);
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, newCameraPos, t);
        }
        void HandleRotation()
        {
            mouseX = Input.GetAxis(StaticStrings.cameraX);
            mouseY = Input.GetAxis(StaticStrings.cameraY);
            if (values.turnSmooth > 0)
            {
                smothx = Mathf.SmoothDamp(smothx, mouseX, ref smotxVelocity, values.turnSmooth);
                smothY = Mathf.SmoothDamp(smothY, mouseY, ref smothyVelocity, values.turnSmooth);
            }
            else
            {
                smothx = mouseX;
                smothY = mouseY;
            }

            lookAngle += smothx * values.y_rotate_speed;
            Quaternion targetRot = Quaternion.Euler(0, lookAngle, 0);
            cameraHandler.transform.rotation = targetRot;
            tiltAngle -= smothY * values.x_rotate_speed;
            tiltAngle = Mathf.Clamp(tiltAngle, values.minAngle, values.maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        }
        public override void PreStartClient()
        {
            base.PreStartClient();
            GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        }

        public void addMp(int value)
        {
            mySkill.addManaPoints(value);
        }

        public void activeFlour()
        {
            CmdActiveFlour();
        }
        [ClientRpc]
        void RpcAtiveFlour()
        {
            if (!isServer)
            {
                canvas.actriveFlour();
            }
        }
        [Command]
        public void CmdActiveFlour()
        {
            canvas.actriveFlour();
            RpcAtiveFlour();
        }

    }
}

public class skillClass
{
    public bool skillIsActive;
    public float skillTime = 15;
    public float skillCounter = 15;
    public int manaPoints=0;

    public void Tick(float d)
    {
        if (!skillIsActive) return;
        skillCounter -= d;
        if (skillCounter <= 0)
        {
            skillIsActive = false;
            skillCounter = skillTime;
            removeManaPoints(100);
        }
    }
    
    public void activeSkill()
    {
       skillIsActive = true;   
    }
    public void addManaPoints(int value)
    {
        manaPoints += value;
    }
    public void removeManaPoints(int v)
    {
        manaPoints -= v;
        if (manaPoints <= 0)
        {
            manaPoints = 0;
        }
    }

}

public class Timer
{
    public float TimeDelay = 15;
    public float TimeCounter = 15;
    public bool go = false;
    public Timer(float delay, float counter)
    {
        TimeCounter = counter;
        TimeDelay = delay;
    }
    public void Tick(float d)
    {
        if (!go) return;
        TimeCounter -= d;
        if (TimeCounter <= 0)
        {
            go = false;
            TimeCounter = TimeDelay;
            
        }
    }

}