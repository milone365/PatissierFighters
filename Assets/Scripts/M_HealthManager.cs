using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class M_HealthManager : NetworkBehaviour,IHealth,IBurn
{
    public NetworkStartPosition respawnPosition;
    public charType sex = charType.male;
    string Deathsound="";
    public event Action<bool> onDeath;
    
    int maxHealth=12;
    public float getHealth() { return healthValue; }
    public float getMaxHealth() { return maxHealth; }
    
    public bool isBurning { get ; set ; }
    public float fireLifeTime { get ; set; }
    public float damageforSecond { get; set ; }
    public float damageDelay { get ; set ; }
    public float damageDelayCounter { get ; set ; }
   
    [SyncVar(hook = "OnChangeHealth")]
    public int healthValue = 10;

    void setup()
    {
        fireLifeTime = 8;
        damageforSecond = 1;
        damageDelayCounter = 1;
        damageDelay = damageDelayCounter;
    }
    private void Start()
    {
      
        switch (sex)
        {
            case charType.male:Deathsound = StaticStrings.deathMale;
                break;
            case charType.feumale:Deathsound= StaticStrings.deathFemale;
                break;
            case charType.robot:
                Deathsound = StaticStrings.robotHurt;
                break;
        }

    }

    private void Update()
    {
        
        if (isBurning)
        {
            getFireDamage();
        }
      
    }
    //ダメージを受ける
    public void takeDamage(float damageToTake)
    {
       CmdChangeHealth((int)-damageToTake);
    }
    //
    public void Healing(float heal)
    {
        healthValue +=(int) heal;
        
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
        if (onDeath != null)
        {
            onDeath(true);
        }
        StartCoroutine(respawning());
    }

    IEnumerator respawning()
    {
        GetComponent<Animator>().SetTrigger(StaticStrings.death);
       yield return new WaitForSeconds(3f);
        teleportToSpawnPoint();
        if (onDeath != null)
        {
            onDeath(false);
        }
        Healing(maxHealth);
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.RESURRECTION);
    }

    public void teleportToSpawnPoint()
    {
        transform.position = respawnPosition.transform.position;
    }
    public void powerUp(float value)
    {
        maxHealth +=(int) value;
        healthValue +=(int) value;

    }

    public void burn()
    {
        if (!isBurning)
        {
            isBurning = true;
        }
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
        }
    }

    public void becameInvincible(bool valuer)
    {

    }
}
