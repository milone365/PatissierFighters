using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
namespace M
{
    public class M_Tower : NetworkBehaviour, ITeam, IHealth, IBurn
    {
        [SerializeField]
        Text pointText = null;
        [SerializeField]
        Transform flag = null;
        public Slider hpBar;
        public List<GameObject> networkTower;
        #region values
        [SyncVar(hook = "OnChangeHealth")]
        public int health = 25;

        int maxHealh = 25;
        BoxCollider b_collider;
        Vector3 flagPos = new Vector3();
        Vector3 flagStartPos = new Vector3();

        public float getMaxHealth()
        {
            return maxHealh;
        }
        public float getHealth()
        {
            return health;
        }
        float height;
        public float Height { get { return height; } set { height = value; } }
        Team towerteam;
        public Team TowerTeam { get { return towerteam; } set { towerteam = value; } }

        public bool isBurning { get; set; }
        public float fireLifeTime { get; set; }
        public float damageforSecond { get; set; }
        public float damageDelay { get; set; }
        public float damageDelayCounter { get; set; }

        public GameObject piece;
        public GameObject basePiece, Burnedpancake;
        bool invulnerability;
        float invulnerabilityTime = 3;
        float invulnerabilityCounter = 3;
        List<GameObject> pieces = new List<GameObject>();

        #endregion

        //最初はヘルパーをスポーンする
        public void Start()
        {
            b_collider = GetComponent<BoxCollider>();
            if (flag != null)
            {
                flagPos = flag.transform.localPosition;
                flagStartPos = flagPos;
            }
           
            Height = 0;
            fireLifeTime = 8;
            damageforSecond = 1;
            damageDelayCounter = 1;
            damageDelay = 1;
            hpBar = GetComponentInChildren<Slider>();
           
            if (hpBar == null) return;
            hpBar.maxValue = maxHealh;
            hpBar.value = health;
            if (pointText != null)
                pointText.text = height.ToString();
        }


        private void Update()
        {
            if (invulnerability)
            {
                invulnerabilityCounter -= Time.deltaTime;
                if (invulnerabilityCounter <= 0)
                {
                    invulnerability = false;
                    invulnerabilityCounter = invulnerabilityTime;
                }
            }
            
            if (isBurning)
            {
                getFireDamage();
            }
        }
        public Vector3 lastBoxHeight()
        {
            Vector3 pos = new Vector3(0, basePiece.transform.localPosition.y + height, 0);
            return pos;
        }

        public Team getTeam()
        {
            return towerteam;
        }
        //大きくなる関数
        public void Healing(float cm)
        {
            int cm2 = (int)cm;
            health += cm2;
            Build(cm2);

        }
        void OnChangeHealth(int n)
        {
            health = n;
            hpBar.value = health;
            
        }
        private void UpdateHealthBar(float value)
        {
            hpBar.value = value; 
        }
        [Command]
        public void CmdChangeHealth(int hitValue)
        {
            health = health + hitValue;
            hpBar.value = health;
            if (health <= 0)
            {
                DestroyTower();
                Rpcdestroy();
            }
            
        }
        //減る関数
        public void takeDamage(float dmg)
        {
          CmdChangeHealth((int)-dmg);
        }
        [ClientRpc]
        void Rpcdestroy()
        {
            if (!isServer)
            {
                DestroyTower();
            }
        }
        public void DestroyTower()
        {
            
            if (Soundmanager.instance != null)
                Soundmanager.instance.PlaySeByName(StaticStrings.towerBreak);
            var a = Enumerable.Range(0, pieces.Count).Reverse();
            foreach (var p in a)
            {
                if (pieces.Count > 0)
                {
                    GameObject newPankake = Instantiate(Burnedpancake, pieces[p].transform.position, Quaternion.identity);
                   
                    Destroy(pieces[p].gameObject);
                    pieces.Remove(pieces[p]);
                    height--;
                    b_collider.size = new Vector3(b_collider.size.x, b_collider.size.y - 1, b_collider.size.z);
                    height = height <= 0 ? 0 : height;
                }

            }

            health = maxHealh;
            if (flag == null) return;
            flagPos = flagStartPos;
            flag.transform.localPosition = flagPos;
            if (pointText != null)
                pointText.text = height.ToString();
        }

        public void Build(int v)
        {

            height += v;
            GameObject newPiece = Instantiate(piece, transform.position, transform.rotation);
            newPiece.transform.SetParent(basePiece.transform);
            newPiece.transform.localPosition = lastBoxHeight();
            newPiece.name = "Piece " + height;
            pieces.Add(newPiece);
            b_collider.size = new Vector3(b_collider.size.x, b_collider.size.y + 1, b_collider.size.z);
            updateFlag(v);
            if (pointText != null)
                pointText.text = height.ToString();
        }
       
        void updateFlag(float valuer)
        {
            if (flag == null) return;
            flagPos = new Vector3(flagPos.x, flagPos.y + valuer, flagPos.z);
            flag.transform.localPosition = flagPos;
        }

        public void burn()
        {
            if (!isBurning)
                isBurning = true;
        }

        public void getFireDamage()
        {
            fireLifeTime -= Time.deltaTime;
            damageDelayCounter -= Time.deltaTime;
            if (damageDelayCounter <= 0)
            {
                damageDelayCounter = damageDelay;
                takeDamage(damageforSecond);
            }
            if (fireLifeTime <= 0)
            {
                isBurning = false;
                fireLifeTime = 8;
                damageDelayCounter = damageDelay;
            }
        }

        public Transform getHand()
        {
            throw new System.NotImplementedException();
        }

    }
}
