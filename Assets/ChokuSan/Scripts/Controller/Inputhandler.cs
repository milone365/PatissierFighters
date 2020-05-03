using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
   
     public class Inputhandler : MonoBehaviour,IFroze,IConfused,ITornado,Iflour
     {
        string playerID="";
        [SerializeField]
        Sprite questionMarker = null;
        [SerializeField]
        float vortexTime = 8;
        [SerializeField]
        ParticleSystem effect = null;
        Vector3 vortexPos;
        Quaternion vortexRotation;
        bool isInTheVortex = false;
        float flyHeight = 1;
        public ControllerMode controller=ControllerMode.pro;
        float horizontal;
        float vertical;
        PlayerActions playerActions;
        public event Action<Transform> onGiveTarget;
        bool isInit;
        B_Player player;
        float delta;
        public StatesManager states;
        Camerahandler camerahandler;
        Animator anim;
        BulletSpawner bs;
        [SerializeField]
        Transform hand = null;
        [SerializeField]
        WPbullet bomb = null;
        float frozingTime = 5;
        float confusedTime = 10;
        bool isConfused = false;
        Rigidbody rb;
        string ver = "", hor = "";

        public bool isfrozen { get; set; }

        bool L2Input()
        {
            return Input.GetButton(playerID+StaticStrings.L2_key);

        }

        public void InitInGame()
        {
            
            anim = GetComponent<Animator>();
            player = GetComponentInChildren<B_Player>();
            player.initializeCanvas();
            //dependency with statemanager
            states.init();
            camerahandler = GetComponentInChildren<Camerahandler>();
            camerahandler.Init(this);
            int gameMode = 0;
            if (PlayerPrefs.HasKey(StaticStrings.GameControllerValue))
            {
                gameMode = PlayerPrefs.GetInt(StaticStrings.GameControllerValue);
            }
            switch (gameMode)
            {
                case 0:controller = ControllerMode.simple;break;
                case 1:controller = ControllerMode.pro; break;
            }
            if (controller == ControllerMode.pro)
            {
                playerActions = new PlayerActions(player, this, anim);
            }
            else
            {
                playerActions = new SimpleAction(player, this, anim);
            }
            
            player.Helper.INITIALIZETARGETEVENT(this);
            reciveTrget(playerActions.targets[0]);
            bs = GetComponentInChildren<BulletSpawner>();
            bs.INITIALIZING(player);
            player.reciveAnimator(anim);
            isInit = true;
            isfrozen = false;
            rb = GetComponent<Rigidbody>();
            playerID = player.getID();


            ver = playerID + StaticStrings.Vertical;
            hor = playerID + StaticStrings.Horizontal;
           /* if (camerahandler != null)
                camerahandler.GetComponent<Transform>().SetParent(null);*/
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

        }

        void GetInput_FixedUpdate()
        {
            vertical = Input.GetAxis(ver);
            horizontal = Input.GetAxis(hor);

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
            if (isConfused)
            {
                confusedTime -= Time.deltaTime;
                if (confusedTime <= 0)
                {
                    isConfused = false;
                    ver = StaticStrings.Vertical;
                    hor = StaticStrings.Horizontal;
                    player.changeIcon(null);
                }
            }
        }
        #endregion
        #region Update
        void Update()
        {
            if (!isInit)
                return;
            if (isfrozen)
            {
                froze();
                return;
            }
            delta = Time.deltaTime;
            states.tick(delta);
            crossAirUpdate();
            playerActions.InputUpdating();
            if (isInTheVortex)
            {
                vorterxUpdate();
            }
        }

        private void froze()
        {
            frozingTime -= Time.deltaTime;
            if (frozingTime <= 0)
            {
                isfrozen = false;
                frozingTime = 5;
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezeRotationZ;
                rb.constraints = RigidbodyConstraints.FreezeRotationX;
            }
        }

        public void shot()
        {

            player.BULLETSPAWNER.shot();

        }

        void crossAirUpdate()
        {
            if (!states.onGround()) return;
            states.isAiming = L2Input();

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
            if(!isfrozen)
            isfrozen = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        public void spawnBomb()
        {
            WPbullet b = Instantiate(bomb, hand.position, hand.rotation);
            Vector3 dir;
            if (Input.GetButton(playerID+StaticStrings.L2_key))
                dir = player.getCameraDir();
            else
            {
                dir = transform.forward * 50;
            }
            b.gameObject.GetComponent<Rigidbody>().AddForce(dir * 50);
        }

        public void confuse()
        {
            confusedTime = 10;
            isConfused = true;
            ver = playerID+StaticStrings.Horizontal;
            hor = playerID+StaticStrings.Vertical;
            player.changeIcon(questionMarker);
        }

        public void Vortex()
        {
            if (isInTheVortex) return;
            isInTheVortex = true;
            vortexPos = transform.position;
            vortexRotation = transform.rotation;
            if (effect != null) effect.Play();
            rb.useGravity = false;
            rb.mass = 0;
            rb.AddTorque(Vector3.up, ForceMode.Impulse);
        }
        void vorterxUpdate()
        {
            transform.Translate(0, vortexPos.y + flyHeight, 0, Space.World);
            flyHeight += Time.deltaTime;
            vortexTime -= Time.deltaTime;
            if (vortexTime <= 0)
            {
                isInTheVortex = false;
                vortexTime = 8;
                rb.useGravity = true;
                rb.mass = 1;
                transform.rotation = vortexRotation;
                transform.position = vortexPos;
                if (effect != null) effect.Stop();
                flyHeight = 1;
            }
        }
        
        public void activeFlour()
        {
            player.getCanvas().activeFlour();
        }
    }

    public enum GamePhase
    {
        inGame, inMenu,
    }

}
public enum ControllerMode
{
    simple,
    pro
}

