using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace td
{
    public class M_BulletSpawner : NetworkBehaviour,IpickUp
    {

        [SerializeField]
        Inventory inventory;
        [SyncVar(hook = "OnChangeBullet")]
        string bulletName="DefaultBullet";
        Dictionary<string, GameObject> BulletList=new Dictionary<string, GameObject>();
        GameObject currentBullet;
        ParticleSystem electric;
        float fireRate = 0.3f;
        float fireCounter = 0.3f;
        [SerializeField]
        protected GameObject weaponBullet = null;
        GameObject bombPrefab;
        [SerializeField]
        Transform spawnPoint = null;
        M_InputHandler handler;
        Camera cameraMain;
        bool isPlayer;
        int ammonation = 0;
        public Transform getSpawnpoint()
        {
            return spawnPoint;
        }
        Transform tg;
        [SerializeField]
        float bulletSpeed = 50;
        M_StateManager statemanager;
        Vector3 direction = new Vector3();
        
        int bombs = 3;
        public Vector3 getDir()
        {
            return direction;
        }
        bool R2Input;
        bool L2_input;
        bool square_input;
        
             
        
        public bool isfrozen { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private void Start()
        {

            
            if (inventory != null)
            {
                foreach (var item in inventory.bullets)
                {
                    BulletList.Add(item.name, item);
                }
            }
            bombPrefab = BulletList["M_bomb"];
            currentBullet = weaponBullet;
            electric = GetComponentInChildren<ParticleSystem>();
            if (!isLocalPlayer) return;
            direction = transform.forward;
            cameraMain = FindObjectOfType<M_Camera>().GetComponentInChildren<Camera>();
            
           
        }

        private void FixedUpdate() {
            R2Input = Input.GetButton(StaticStrings.R2_key);
            L2_input = Input.GetButton(StaticStrings.L2_key);
            square_input= Input.GetButtonDown(StaticStrings.Square_key);
        }
        private void Update()
        {
            if (!isLocalPlayer) return;
            aiming();
            if (R2Input||Input.GetKey(KeyCode.Space))
            {
                fire();
            }
            if (square_input)
            {
                tossBomb();
            }

        }
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
            GetComponent<Animator>().SetTrigger(StaticStrings.shooting);
            GetComponent<SetupLocalPlayer>().CmdChangeAnimState(StaticStrings.shooting);
            CmdShoot();
        }

        private void tossBomb()
        {
           GetComponent<Animator>().SetTrigger(StaticStrings.bomb);
            GetComponent<SetupLocalPlayer>().CmdChangeAnimState(StaticStrings.bomb);
        }
        #region shoot
        [ClientRpc]
        void RpcCreateBullet()
        {
            if (!isServer)
            {
                createBullet();
            }
                
        }
        void createBullet()
        {
            GameObject newBullet = Instantiate(currentBullet, spawnPoint.position, spawnPoint.rotation).gameObject;
            newBullet.GetComponent<LastBuletDefinitive>().getDirection(direction);
            if (ammonation > 0)
            {
                ammonation--;
                if (ammonation <= 0)
                {
                    currentBullet = weaponBullet;
                }
            }
            Destroy(newBullet, 3);
   
        }
        [Command]
        public void CmdShoot()
        {
            createBullet();
            RpcCreateBullet();
        }
        public void goToSpecialShoot(GameObject special)
        {
            currentBullet = special;
            ammonation = 50;
        }

        #endregion
        #region aiming
        void aiming() {
            if (L2_input|| Input.GetKey(KeyCode.A)) {

                Ray ray = new Ray(cameraMain.transform.position, cameraMain.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    Vector3 newDir = hit.point;
                    direction = newDir - transform.position;
                    direction = direction.normalized;
                }
            }
            else 
            {
               direction = Vector3.zero;
            }
        }
        #endregion
        #region Bomb
        public void spawnBomb()
        {
            CmdBomb();
        }
        [ClientRpc]
        void RpcBomb()
        {
            if (!isServer)
            {
                bomb();
            }
        }
        [Command]
        void CmdBomb()
        {
            bomb();
            RpcBomb();
        }
        public void bomb ()
        {
            if (bombs > 0)
            {
                bombs--;
                GameObject newBullet = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
                newBullet.GetComponent<LastBuletDefinitive>().Toss(direction);

            }
        }
        #endregion

        void OnChangeBullet(string s)
        {
            if (isLocalPlayer) return;
            UpdateBulletState(s);
            
        }

        void UpdateBulletState(string sname)
        {
            bulletName = sname;
            if (BulletList.ContainsKey(bulletName))
            {
                currentBullet = BulletList[sname].gameObject;
            }
            ammonation = 50;

        }
        [Command]
        public void CmdChangeBullet(string s)
        {
            UpdateBulletState(s);
            RpcBulletUpdating(s);
        }
        [ClientRpc]
        void RpcBulletUpdating(string s)
        {
            if (!isServer)
            {
                UpdateBulletState(s);
            }
        }

        public void take(int value)
        {
            bombs += value;
        }
    }

}
