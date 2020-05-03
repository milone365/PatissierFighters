using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace td
{
    public class TD_HealthManager : MonoBehaviour,IHealth
    {
       
        
        float health = 10;
        float maxHealth = 10;
       public float getHealth() { return health; }
        public float getmaxHealth() { return maxHealth;
        }
        bool isDead = false;
        TD_GameManager gm=null;
        TD_Status stat;
        private void Start()
        {
            gm = FindObjectOfType<TD_GameManager>();
            TD_Ally ally = GetComponent<TD_Ally>();
            if (ally == null) return;
            stat = ally.status;
            giveValues(stat.health);
         
            
        }
        //リスポーン
        public void respawn()
        {
            if (EffectDirector.instance != null)
            {
                EffectDirector.instance.playInPlace(transform.position, StaticStrings.RESURRECTION);
            }
            isDead = false;
            if (health != maxHealth)
            {
                health = maxHealth;
            }
        }
       
        public void giveValues(float v)
        {
            maxHealth = v;
            health = v;
        }
        public void totalRestore()
        {
            health = maxHealth;
        }
      //ダメージ、死
        public void takeDamage(float v)
        {
            if (isDead) return;
            health -= v;
            if (health <= 0)
            {
                isDead = true;
                if (EffectDirector.instance != null)
                {
                    EffectDirector.instance.playInPlace(transform.position,StaticStrings.SKULL);
                }
                health = 0;
                TD_Character c = GetComponent<TD_Character>();
                if (c != null)
                {
                    //ボールをなくす
                    if (c.getBall())
                    {
                        c.haveBall(false);
                        TD_PancakeBall.instance.loseBall();
                    }
                    c.Dead(true);
                }

            }
        }

      
        public void Healing(float heal)
        {
            throw new NotImplementedException();
        }
    }

}
