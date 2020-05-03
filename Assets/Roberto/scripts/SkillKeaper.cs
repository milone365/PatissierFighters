using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using td;
using System;

public class SkillKeaper : MonoBehaviour
{
    public GameObject weaponModel;
    public Ammonation specialWeapon;
    static int manaPoint = 0;
    TD_StateManager statemanager;
    TD_BulletSpawner spawner;
    ParticleSystem electricity;
    Ammonation plin;
    [SerializeField]
    Transform plinSpawner = null;
    [SerializeField]
     Image manaImage = null;
    public ParticleSystem aura;
    void Start()
    {
        statemanager = GetComponent<TD_StateManager>();
        spawner = GetComponentInChildren<TD_BulletSpawner>();
        plin = new Ammonation();
        
    }

   //input system
    void Update()
    {
        if (Input.GetButtonDown(StaticStrings.Triangle_key))
        {
            ActiveSkill();
        }
        if (Input.GetButtonDown(StaticStrings.R1_key))
        {
            statemanager.changeBombType();
        }
        if (Input.GetButtonDown(StaticStrings.Circle_key))
        {
            usePlin();
        }
    }

    void usePlin()
    {
        if (plin.Ammo <= 0) return;
        plin.Ammo--;
        Instantiate(plin.obj, plinSpawner.position, Quaternion.identity);
    }
    //Pass prefab and ammo 
    internal void addPlin(Ammonation a)
    {
        plin.obj = a.obj;
        plin.Ammo = a.Ammo;
    }

    //スキル
    void ActiveSkill()
    {
        if (manaPoint > 99)
        {
            manaPoint = 0;
            spawner.changeBullet(specialWeapon);
            if (manaImage == null) return;
            manaImage.fillAmount = manaPoint;
            if (aura != null)
                aura.Play();
            weaponModel.SetActive(false);
            specialWeapon.obj.SetActive(true);
            spawner.canvas.changeWeaponImage(true);
        }
    }
    public void deativeAura()
    {
        if (aura != null)
            aura.Stop();
        weaponModel.SetActive(true);
        specialWeapon.obj.SetActive(false);
    }
  //マナ追加
   public void addMp(int value)
    {
        manaPoint += value;
        if (manaPoint > 100)
        {
            manaPoint = 100;
        }
        if (manaImage == null) return;
        manaImage.fillAmount = manaPoint/100;   
   }
}

