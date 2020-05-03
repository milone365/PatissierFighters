using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SV_EnemyHealth : MonoBehaviour, IHealth
{
    public event Action onDeath;
    [SerializeField]
    float health = 10;
    bool isDead = false;
    SV_EnemyManager manager;
    [SerializeField]
    int dropRate = 90;
    [SerializeField]
    int score = 1;
    int startingHealth;
    SkillKeaper keaper;
    public void Healing(float heal)
    {
        throw new NotImplementedException();
    }
    public void passManager(SV_EnemyManager em)
    {
        manager = em;
        startingHealth = (int)health;
        keaper = FindObjectOfType<SkillKeaper>();
    }
    public void takeDamage(float damageToTake)
    {
        if (isDead) return;
        health -= damageToTake;
        if (health <= 0)
        {
            isDead = true;
            keaper.addMp(startingHealth);
            manager.addKillCount(score);
            if (EffectDirector.instance != null)
            {

                int rnd = UnityEngine.Random.Range(0, 101);

                if (rnd >= dropRate)
                {
                    int rnd2 = UnityEngine.Random.Range(0, EffectDirector.instance.objToSpawn.Length);
                    EffectDirector.instance.spawnInPlace(transform.position, rnd2);
                }
            }
            if (onDeath != null)
            {
                int rnd = UnityEngine.Random.Range(1, 3);
                if (rnd == 2)
                {
                    onDeath();
                }
                else
                {
                    if (EffectDirector.instance != null)
                    {
                        EffectDirector.instance.playInPlace(transform.position, StaticStrings.blood);
                        Destroy(gameObject);
                    }

                }
            }
            else
            {
                if (EffectDirector.instance != null)
                {
                    EffectDirector.instance.playInPlace(transform.position, StaticStrings.blood);
                   
                }

                Destroy(gameObject);
            }


            }
        }
}

