using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
namespace td
{
    public class M_TD_HealthManager : NetworkBehaviour,IHealth
    {

        public NetworkStartPosition respawnPosition;
        public charType sex = charType.male;
        string Deathsound = "";
        Animator anim;

        int maxHealth = 12;
        public float getHealth() { return healthValue; }
        public float getMaxHealth() { return maxHealth; }

        public bool isBurning { get; set; }
        public float fireLifeTime { get; set; }
        public float damageforSecond { get; set; }
        public float damageDelay { get; set; }
        public float damageDelayCounter { get; set; }
        [SyncVar(hook = "OnChangeAnimation")]
        public string animState = "idle";
        [SyncVar(hook = "OnChangeHealth")]
        public int healthValue = 10;
        MU_TD_GameManager gm=null;
        TD_Status stat;
        private void Start()
        {
            gm = FindObjectOfType<MU_TD_GameManager>();
            M_TD_Ally ally = GetComponent<M_TD_Ally>();
            if (ally == null) return;
            stat = ally.status;
            giveValues(stat.health);
            anim = GetComponent<Animator>();
            
        }
      
        public void giveValues(float v)
        {
            maxHealth = (int)v;
            healthValue = (int) v;
        }
     
        //ダメージを受ける
        public void takeDamage(float damageToTake)
        {
            CmdChangeHealth((int)-damageToTake);
        }
        //
        public void Healing(float heal)
        {
            healthValue += (int)heal;

            if (healthValue >= maxHealth)
            {
                healthValue = maxHealth;

            }

        }
        void OnChangeHealth(int n)
        {
            healthValue = n;

        }
        [Command]
        public void CmdChangeHealth(int hitValue)
        {
            healthValue = healthValue + hitValue;
            if (healthValue <= 0)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.SKULL);
                if (Soundmanager.instance != null)
                    Soundmanager.instance.PlaySeByName(Deathsound);
                healthValue = 0;
                death();
            }
            RpcTakingDamage(hitValue);

        }

        [ClientRpc]
        void RpcTakingDamage(int v)
        {
            if (!isServer)
            {
                healthValue = healthValue + v;
                if (healthValue <= 0)
                {
                    EffectDirector.instance.playInPlace(transform.position, StaticStrings.SKULL);
                    if (Soundmanager.instance != null)
                        Soundmanager.instance.PlaySeByName(Deathsound);

                    healthValue = 0;
                    death();
                }
            }
        }
        public virtual void death()
        {
            M_TD_Character c = GetComponent<M_TD_Character>();
            if (c != null)
            {

                if (c.getBall())
                {
                    c.haveBall(false);
                    M_TD_PancakeBall.instance.loseBall();
                }
                c.Dead(true);
            }
            
            StartCoroutine(respawning());
        }

        IEnumerator respawning()
        {
            CmdChangeAnimState(StaticStrings.death);
            yield return new WaitForSeconds(3f);
            teleportToSpawnPoint();
            Healing(maxHealth);
            EffectDirector.instance.playInPlace(transform.position, StaticStrings.RESURRECTION);
        }

        public void teleportToSpawnPoint()
        {
            transform.position = respawnPosition.transform.position;
        }

        public void respawn()
        {
            teleportToSpawnPoint();
        }
        void OnChangeAnimation(string s)
        {
            UpdateAnimationState(s);
        }

        [Command]
        public void CmdChangeAnimState(string s)
        {
            UpdateAnimationState(s);
            RpcAnimateUpdating(s);
        }
        [ClientRpc]
        void RpcAnimateUpdating(string s)
        {
            if (!isServer)
            {
                UpdateAnimationState(s);
            }
        }
        void UpdateAnimationState(string s)
        {
            // if (animState == s) return;
            anim.SetTrigger(s);
        }
    }


}
