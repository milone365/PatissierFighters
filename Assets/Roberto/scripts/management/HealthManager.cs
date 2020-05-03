using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour,IHealth,IBurn
{
    public charType sex=charType.male;
    string Deathsound="";
    public event Action<bool> onDeath;
    float health = 12;
    float maxHealth=12;
    public float getHealth() { return health; }
    public float getMaxHealth() { return maxHealth; }
    bool isDead = false;
    Vector3 spawnPosition=new Vector3();

    public bool isBurning { get ; set ; }
    public float fireLifeTime { get ; set; }
    public float damageforSecond { get; set ; }
    public float damageDelay { get ; set ; }
    public float damageDelayCounter { get ; set ; }
    bool isInvincible = false;
    

    void setup()
    {
        fireLifeTime = 8;
        damageforSecond = 1;
        damageDelayCounter = 1;
        damageDelay = damageDelayCounter;
    }
    //キャラクターによってサウンドは違う
    private void Start()
    {
      spawnPosition= transform.position;
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
        if (isDead||isInvincible) return;
        health -= damageToTake;
        if (health <= 0)
        {
            EffectDirector.instance.playInPlace(transform.position, StaticStrings.SKULL);
            if(Soundmanager.instance!=null)
            Soundmanager.instance.PlaySeByName(Deathsound);
            isDead = true;
            health = 0;
            death();
        }
       
    }
    //
    public void Healing(float heal)
    {
        health += heal;
        
        if (health >= maxHealth)
        {
            health = maxHealth;
           
        }
        
    }
    
    public virtual void death()
    {
        StartCoroutine(respawning());
    }

    //リスポーン
    IEnumerator respawning()
    {
        if (onDeath != null)
        onDeath(true);
        yield return new WaitForSeconds(3f);
        teleportToSpawnPoint();
        yield return new WaitForSeconds(2f);
        isDead = false;
        if (onDeath != null)
            onDeath(false);
        Healing(maxHealth);
        EffectDirector.instance.playInPlace(transform.position, StaticStrings.RESURRECTION);
    }

    public void teleportToSpawnPoint()
    {
        transform.position = spawnPosition;
    }
    public void powerUp(float value)
    {
        maxHealth += value;
        health += value;

    }

    //

    public void burn()
    {
        if (!isBurning)
        {
            isBurning = true;
        }
    }

    //火のダメージ
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

    public void becameInvincible(bool v)
    {
        isInvincible = v;
    }
}

public enum charType
{
    male,
    feumale,
    robot,
}
