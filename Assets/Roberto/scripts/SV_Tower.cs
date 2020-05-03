using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SV_Tower : MonoBehaviour,IHealth
{
    [SerializeField]
    float maxHealth = 100;
    float health = 100;
    [SerializeField]
    Slider hpBar = null;
    Text hptext = null;
    [SerializeField]
    GameObject[] pancakePicies=null;
    private void Start()
    {
        health = maxHealth;
        hpBar = GameObject.Find("HealthHP").GetComponent<Slider>();
        if (hpBar != null)
        {
            hpBar.maxValue = maxHealth;
        }
        hptext = hpBar.GetComponentInChildren<Text>();
    }
    //削除されたflag
    public bool getIsDestroyed()
    {
        return isDeatroyed;
    }
    bool isDeatroyed = false;

    //Hpバーアップデート
    private void Update()
    {
        if (hpBar == null) return;
        hpBar.value = health;
        hptext.text = health + "/" + maxHealth;
    }
    public void takeDamage(float damage)
    {
        if (isDeatroyed) return;
        health -= damage;
       
        if (health <= 0)
        {
            isDeatroyed = true;
            GameOver();
        }
        else
        {
            //パンケーキをピースを削除する
            if (health % 10 == 0)
            {
                pancakePicies[(int)health / 10].SetActive(false);
            }
        }
        
    }
    //回復
    public void Healing(float hp)
    {
        health += hp;
        foreach(var p in pancakePicies)
        {
            if (!p.activeInHierarchy)
            {
                p.SetActive(true);
                break;
            }
        }
        
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
      
    }
    //終了
    public void GameOver()
    {
        SV_GameManager gm = FindObjectOfType<SV_GameManager>();
        gm.GoToEnd();
    }
}
