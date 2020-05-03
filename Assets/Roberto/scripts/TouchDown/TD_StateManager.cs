using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SA;

//stateManager に基づいて作られたスクリプト

namespace td
{
    public class TD_StateManager : MonoBehaviour
    { 
        #region values
        Vector3 vortexPos;
        Quaternion vortexRotation;
        bool isInTheVortex = false;
        [SerializeField]
        Transform hand = null;
        public Camera FollowCamera;
        public STATUS status = null;
        CapsuleCollider coll = null;
        public ControllerStats stats;
        public Animator anim;
        [HideInInspector]
       public bool isAiming, isRunning;
        Rigidbody rig;
        Transform thisTr;
        bool initialized;
        [SerializeField]
        float jumpForce = 50;
        public float originYoffset = 0.6f;
        public float distance = 0.7f;
        Vector3 dir;
        float gravityScale = 10f;
        public string playerId="P1_";
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
        TD_PlayerControler player;
        public Team tm;
        public TD_BulletSpawner BULLETSPAWNER { get; set; }
        public float changeSpeedTime { get; set; }
        public float changeSpeedCounter { get; set; }
        #region setUp_init
        [SerializeField]
        ParticleSystem effect = null;
        float vortexTime = 8;
        int bombIndex=0;
        public BOMB candyBomb;
        //[HideInInspector]
        public List<BOMB> allBombs = new List<BOMB>();
        Text bombtext;
        #endregion
        public void init()
        {
            status = new STATUS();
            BULLETSPAWNER = GetComponentInChildren<TD_BulletSpawner>();
            anim = GetComponent<Animator>();
            thisTr = this.transform;
            rig = GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            coll = GetComponent<CapsuleCollider>();
            BULLETSPAWNER.INITIALIZING(this);
            allBombs.Add(candyBomb);
            bombtext = GameObject.Find("BombBox").GetComponentInChildren<Text>();

        }


        public bool onGround()
        {
            return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + 0.1f);
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
            if (isInTheVortex)
            {
                vorterxUpdate();
                return;
            }
            if (isAiming)
            {
                MovementAiming();
            }
            else
            {
                MovementNormal();
            }
            rotationNormal();
            isRunning = Input.GetButton(playerId + StaticStrings.L1_key);
        }
        #endregion


        #region normal
        void MovementNormal()
        {

            float yStore = dir.y;
            float speed = status.Speed;
            if (isRunning) { speed = status.RunSpedd; }

            dir = thisTr.forward * (speed * inp.moveAmount);

            dir.y = yStore;
            if (onGround())
            {
                dir.y = 0;
                if (Input.GetButtonDown(playerId + StaticStrings.X_key))
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
            anim.SetTrigger(StaticStrings.Jump);
            dir.y = jumpForce;
        }

        public Transform getHand()
        {
            return hand;
        }
        public void shot()
        {
            BULLETSPAWNER.shoot();
        }
        public void spawnBomb()
        {
            if (hand == null) return;
            if (allBombs[bombIndex].Ammo > 0)
            {
                allBombs[bombIndex].useAmmo();
                
               WPbullet b = Instantiate(allBombs[bombIndex].bombPrefab, getHand().position, getHand().rotation);
                Vector3 dir;
                if (Input.GetButton(playerId + StaticStrings.L2_key))
                    dir = getCameraDir();
                else
                {
                    dir = transform.forward * 50;
                }
                b.gameObject.GetComponent<Rigidbody>().AddForce(dir * 50);
                if (bombtext != null)
                    bombtext.text = allBombs[bombIndex].Ammo.ToString();
            }
        }
        public Vector3 getCameraDir()
        {
            if (FollowCamera != null)
                return FollowCamera.transform.forward * 50;
            else
            {
                return Vector3.zero;
            }
        }

        public void Vortex()
        {
            if (isInTheVortex) return;
            isInTheVortex = true;
            vortexPos = transform.position;
            vortexRotation = transform.rotation;
            if (effect != null) effect.Play();
            rig.isKinematic = false;
            rig.useGravity = false;
            rig.mass = 0;
            rig.constraints = RigidbodyConstraints.None;
            rig.AddTorque(Vector3.up, ForceMode.Impulse);
        }

        void vorterxUpdate()
        {
            transform.Translate(0, vortexPos.y + 2 * Time.deltaTime, 0, Space.World);
            vortexTime -= Time.deltaTime;
            if (vortexTime <= 0)
            {
                isInTheVortex = false;
                vortexTime = 8;
                rig.isKinematic = true;
                rig.useGravity = true;
                rig.mass = 1;
                rig.constraints = RigidbodyConstraints.FreezeAll;
                transform.rotation = vortexRotation;
                if (effect != null) effect.Stop();

            }
        }

        public void changeBombType()
        {
            bombIndex++;
            bombIndex %= allBombs.Count;
            if (bombtext != null)
                bombtext.text = allBombs[bombIndex].Ammo.ToString();

        }

        public void addBomb(BOMB b)
        {
            foreach(var item in allBombs)
            {
                if (b.bombPrefab == item.bombPrefab)
                {
                    item.addAmmo(b.Ammo);
                 
                }
            }
            if (bombtext != null)
                bombtext.text = allBombs[bombIndex].Ammo.ToString();
        }
    }


}


[System.Serializable]
public class BOMB
{
    public WPbullet bombPrefab;
    public int Ammo;
    public void useAmmo()
    {
        Ammo--;
        if (Ammo <= 0)
        {
            Ammo = 0;
        }
    }
    public void addAmmo(int value)
    {
        Ammo += value;
    }
}