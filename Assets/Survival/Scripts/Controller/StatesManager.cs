using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    public class StatesManager : MonoBehaviour,ITeam,IJump
    {
        string playerID = "";
        CapsuleCollider coll=null;
        public ControllerStats stats;
        [HideInInspector]
        public bool isAiming,isRunning;
        public Animator anim;
        Rigidbody rig;
        Transform thisTr;
        bool initialized;
        BulletSpawner bulletSpawner = null;
        public BulletSpawner BULLETSPAWNER { get { return bulletSpawner; } set { bulletSpawner = value; } }
        [SerializeField]
        float jumpForce = 10;
        public float originYoffset=0.6f;
      //  public float distance = 0.7f;
        Vector3 dir;
        float gravityScale = 10f;

        Vector3 colliderSize()
        {
            Vector3 size = new Vector3(coll.bounds.center.x,
                coll.bounds.min.y, coll.bounds.center.z);
            return size;
        }
        public Transform getTransform()
        {
            return thisTr;
        }
        public InputVariables inp;
        public float delta;
        Vector3 v;
        //  public Transform referenceParent;
        public LayerMask groundLayer;
        B_Player player;
        Team tm;

        public float changeSpeedTime { get ; set ; }
        public float changeSpeedCounter { get ; set ; }
        #region setUp_init
        public void init()
        {
            player = GetComponentInChildren<B_Player>();
            anim = GetComponent<Animator>();
            thisTr = this.transform;
            rig = GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            tm = player.team;
            coll = GetComponent<CapsuleCollider>();
            playerID = player.getID();
        }


       public bool onGround()
        {
           return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + originYoffset);
        }
        #endregion

        #region update
        public void tick(float d)
        {
            delta = d;
           handleAnimationAll();
           
        }
        void handleAnimationAll()
        {
           if (isAiming)
            {
                HandleAnimationAiming();
            }
            else
            {
                handleAnimationNormal();
            }
        }
        #endregion
        #region fixedUpdate
        public void FixedTick(float d)
        {
            delta = d;
            
           if (isAiming)
            {
                MovementAiming();
            }
            else
            {
                MovementNormal();
            }
            rotationNormal();
            isRunning = Input.GetButton(playerID+"Run");
            
        }
        #endregion


        #region normal
        void MovementNormal()
        {
          
            float yStore = dir.y;
            float speed = player.status.Speed;
            
            if (isRunning) { speed = player.status.RunSpedd; }
            
            dir = thisTr.forward * (speed * inp.moveAmount);
            
            dir.y = yStore;
            if (onGround())
            {
                dir.y = 0;
                if (Input.GetButtonDown(playerID+StaticStrings.X_key))
                {
                    jump();
                }
            }
            dir.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            rig.velocity = dir;
        }
        void rotationNormal()
        {
            if (!isAiming)
                inp.rotateDirection = inp.moveDirection;
            Vector3 targetDir = inp.rotateDirection;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = thisTr.forward;
            Quaternion lookRot = Quaternion.LookRotation(targetDir);
            Quaternion targetRot = Quaternion.Slerp(thisTr.rotation, lookRot, stats.rotateSpeed * delta);
            thisTr.rotation = targetRot;
        }
        void handleAnimationNormal()
        {
            float anim_v = inp.moveAmount;
            anim.SetFloat(StaticStrings.move, anim_v, 0.15f, delta);
        }
        #endregion
        #region aiming
        void MovementAiming()
        {
          float speed = stats.aimSpeed;
          v = inp.moveDirection * speed;
          rig.velocity = v;
        }

        void HandleAnimationAiming()
        {
            float v = inp.vertical;
            float h = inp.horizontal;
            //anim.SetFloat("horizontal", h, 0.2f, delta);
            //anim.SetFloat("vertical", v, 0.2f, delta);
        }
      
        public Team getTeam()
        {
            return tm;
        }

        public void speedDown()
        {
            throw new System.NotImplementedException();
        }

        public void speedUp()
        {
            throw new System.NotImplementedException();
        }
        #endregion


        public void jump()
        {
            anim.SetTrigger(playerID+StaticStrings.Jump);
            dir.y = jumpForce;
        }

        public Transform getHand()
        {
            throw new System.NotImplementedException();
        }
    }
    
   
}
    [System.Serializable]
public class InputVariables
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public Vector3 moveDirection;
    public Vector3 aimPosition;
    public Vector3 rotateDirection;
}



    


